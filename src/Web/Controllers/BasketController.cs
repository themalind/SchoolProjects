using Logic.Entities;
using Logic.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Web.Extensions;
using Web.Models;
namespace Web.Controllers;

public class BasketController : Controller
{
    private readonly ILogger<BasketController> _logger;
    private readonly IRestaurantService _restaurantService;
    private readonly IOrderService _orderService;

    public BasketController(
        ILogger<BasketController> logger,
        IRestaurantService restaurantService,
        IOrderService orderService)
    {
        _logger = logger;
        _restaurantService = restaurantService;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index(CancellationToken token)
    {
        var basketViewModel = new BasketViewModel();

        await FillBasketViewModelAsync(basketViewModel, token);

        return View(basketViewModel);
    }

    private async Task FillBasketViewModelAsync(BasketViewModel basketViewModel, CancellationToken token)
    {
        var basket = HttpContext.Session.Get<Basket>("Basket") ?? new Basket(new List<BasketItem>());

        Dictionary<string, decimal> deliveryFees = [];

        foreach (var item in basket.BasketItems)
        {
            var course = await _restaurantService.GetFoodCourseByIdentifierAsync(item.FoodCourseIdentifier, token) ?? throw new ArgumentNullException();

            var restaurant = await _restaurantService.GetByIdentifierAsync(course.RestaurantIdentifier, token) ?? throw new ArgumentNullException();

            deliveryFees.TryAdd(restaurant.Name, restaurant.DeliveryFee);
        }

        basket.AddDeliveryFee(deliveryFees.Values.Sum());

        HttpContext.Session.Set("Basket", basket);

        var items = await Task.WhenAll(basket.BasketItems.Select(async item =>
            new BasketItemViewModel
            {
                FoodCourseIdentifier = item.FoodCourseIdentifier,
                RestaurantIdentifier = item.RestaurantIdentifier,
                ProductName = (await _restaurantService.GetFoodCourseByIdentifierAsync(item.FoodCourseIdentifier, token))?.Name ?? "Unknown product",
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }
        ));

        basketViewModel.Items = items.ToList();
        basketViewModel.DeliveryFee = deliveryFees;
        basketViewModel.TotalPrice = basket.BasketItems.Sum(item => item.Quantity * item.UnitPrice);
    }

    [HttpPost]
    [ActionName("Index")]
    public async Task<IActionResult> CreateOrder(BasketViewModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            await FillBasketViewModelAsync(model, token);
            return View(model);
        }

        var basket = HttpContext.Session.Get<Basket>("Basket") ?? new Basket(new List<BasketItem>());

        var orderItems = await Task.WhenAll(basket.BasketItems.Select(async oi =>
        {
            var foodCourse = await _restaurantService.GetFoodCourseByIdentifierAsync(oi.FoodCourseIdentifier, token);

            if (foodCourse is null)
            {
                throw new ArgumentNullException(nameof(foodCourse), "Foodcourse not found!");
            }

            return new OrderItem(
            foodCourse.Name,
            oi.Quantity,
            oi.UnitPrice,
            oi.RestaurantIdentifier,
            oi.FoodCourseIdentifier
           );

        }));

        var restaurantIdentifier = basket.BasketItems.Select(c => c.RestaurantIdentifier).FirstOrDefault() ?? throw new ArgumentNullException();

        if (!await IsStillOpenAsync(restaurantIdentifier, token))
        {
            basket.ClearBasket();

            HttpContext.Session.Set("Basket", basket);

            await FillBasketViewModelAsync(model, token); // Kommer retunera en tom varukorg till vyn

            ModelState.AddModelError("", "Köket är nu stängt!");
            return View(model);
        }

        Order order = new(
            new Customer(model.CreateOrderData.FullName!, model.CreateOrderData.PhoneNumber!, model.CreateOrderData.Email!),
            new Address(model.CreateOrderData.StreetAddress!, model.CreateOrderData.City!, model.CreateOrderData.ZipCode!),
            orderItems.ToList(),
            basket.Deliveryfee);

        var createdOrder = await _orderService.AddAsync(order, token);

        HttpContext.Session.Set("CreatedOrder", createdOrder);

        return RedirectToAction("OrderConfirmation", "Order");
    }

    private async Task<bool> IsStillOpenAsync(string restaurantIdentifier, CancellationToken token)
    {
        var currentTime = TimeOnly.FromDateTime(DateTime.UtcNow.ToLocalTime());
        var today = DateTime.UtcNow.DayOfWeek;

        var restaurant = await _restaurantService.GetByIdentifierAsync(restaurantIdentifier, token) ?? throw new ArgumentNullException();

        var openingHours = restaurant.OpeningHours.Where(oh => oh.Day.ToString() == today.ToString()).FirstOrDefault();
        if (openingHours is null)
        {
            return false;
        }

        if (openingHours.KitchenCloseTime > currentTime)
        {
            return true;
        }
        return false;
    }

    public IActionResult RemoveFromBasket(RemoveBasketItemViewModel model)
    {
        var basket = HttpContext.Session.Get<Basket>("Basket") ?? new Basket(new List<BasketItem>());

        var basketItem = basket.BasketItems.Where(b => b.FoodCourseIdentifier == model.FoodCourseIdentifier).FirstOrDefault();

        if (basketItem is null) { return NotFound(); }

        basket.RemoveFromBasket(basketItem);

        HttpContext.Session.Set("Basket", basket);

        return RedirectToAction("Index"); // GET index
    }

    public IActionResult UpdateQuantity(UpdateBasketItemViewModel model)
    {
        var basket = HttpContext.Session.Get<Basket>("Basket") ?? new Basket(new List<BasketItem>());

        var basketItem = basket.BasketItems.Where(b => b.FoodCourseIdentifier == model.FoodCourseIdentifier).FirstOrDefault();

        if (basketItem is null) { return NotFound(); }

        if (model.Action is "increase")
        {
            if (basketItem.Quantity + 1 <= 10)
                basketItem.Quantity++;
        }
        if (model.Action is "decrease" && basketItem.Quantity >= 1)
        {
            basketItem.Quantity--;
        }
        if (basketItem.Quantity == 0)
        {
            basket.RemoveFromBasket(basketItem);
        }

        HttpContext.Session.Set("Basket", basket);

        return RedirectToAction("Index");
    }
}