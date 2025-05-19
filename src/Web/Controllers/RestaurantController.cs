using Logic.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Web.Extensions;
using Web.Models;
namespace Web.Controllers;

public class RestaurantController : Controller
{
    private readonly ILogger<RestaurantController> _logger;
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(
        ILogger<RestaurantController> logger,
        IRestaurantService service)
    {
        _logger = logger;
        _restaurantService = service;
    }

    public async Task<IActionResult> IndexAsync(CancellationToken token)
    {
        var list = await _restaurantService.ListAsync(token);

        if (list is null) { return NotFound(); }

        List<RestaurantIndexViewModel> viewModel = [];

        foreach (var item in list)
        {
            var dto = RestaurantIndexViewModel.Create(item);
            viewModel.Add(dto);
        }

        return View(viewModel);
    }

    public async Task<IActionResult> DetailsAsync(string id, CancellationToken token)
    {
        var restaurant = await _restaurantService.GetByIdentifierAsync(id, token);

        if (restaurant == null)
        {
            return NotFound();
        }

        var viewModel = RestaurantDetailsViewModel.Create(restaurant);

        return View(viewModel);
    }

    public async Task<IActionResult> CourseAsync(string id, CancellationToken token)
    {
        var course = await _restaurantService.GetFoodCourseByIdentifierAsync(id, token);

        if (course is null)
        {
            return NotFound();
        }

        var basket = HttpContext.Session.Get<Basket>("Basket") ?? new Basket(new List<BasketItem>());

        var viewModel = CourseViewModel.MapToCourseViewModel(course);

        viewModel.IsTheSameRestaurant = basket.BasketItems.All(ri => ri.RestaurantIdentifier == course.RestaurantIdentifier);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBasket(CourseViewModel model, CancellationToken token)
    {
        var course = await _restaurantService.GetFoodCourseByIdentifierAsync(model.AddToBasketViewModel.CourseIdentifier, token);

        if (course is null)
        {
            return NotFound();
        }

        var basket = HttpContext.Session.Get<Basket>("Basket") ?? new Basket(new List<BasketItem>());

        if (!ModelState.IsValid)
        {
            model.FillCourseViewModel(course);
            model.IsTheSameRestaurant = basket.BasketItems.All(ri => ri.RestaurantIdentifier == course.RestaurantIdentifier);

            return View("Course", model);
        }

        if (!basket.BasketItems.All(bi => bi.RestaurantIdentifier == course.RestaurantIdentifier))
        {
            basket.ClearBasket();
        }

        basket.AddBasketItem(new BasketItem(model.AddToBasketViewModel.CourseIdentifier, course.RestaurantIdentifier, model.AddToBasketViewModel.Quantity, course.UnitPrice));

        HttpContext.Session.Set("Basket", basket);

        return RedirectToAction("Index", "Basket");
    }
}