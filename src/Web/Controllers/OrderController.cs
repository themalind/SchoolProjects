using Logic.Entities;
using Logic.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Web.Extensions;
using Web.Models;
namespace Web.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }
    public IActionResult OrderConfirmation()
    {
        var order = HttpContext.Session.Get<Order>("CreatedOrder");

        if (order is null)
        {
            throw new ArgumentNullException("Error, order is null!");
        }

        var viewModel = OrderViewModel.Create(order);

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult GetOrderDetails()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> OrderDetails(GetOrderViewModel model, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            return View("GetOrderDetails", model);
        }

        var order = await _service.GetByOrderNumberAsync(model.OrderNumber, token);

        if (order is null ||
               order.Customer.Email != model.Email ||
               order.Customer.PhoneNumber != model.PhoneNumber ||
               !order.Customer.FullName.Equals(model.FullName, StringComparison.InvariantCultureIgnoreCase))
        {
            ModelState.AddModelError("", "Ingen matchande order hittades. Kontrollera att all information är korrekt.");
            return View("GetOrderDetails", model);
        }

        var viewModel = OrderViewModel.Create(order);

        return View(viewModel);
    }

    public async Task<IActionResult> CancelOrder(CancelOrderViewMoel model, CancellationToken token)
    {
        var order = await _service.GetByOrderNumberAsync(model.OrderNumber, token) ?? throw new ArgumentNullException();

        if (order.Status != Logic.Enums.OrderStatus.Received &&
        order.Status != Logic.Enums.OrderStatus.Confirmed &&
        order.Status != Logic.Enums.OrderStatus.CourierAccepted
        )
        {
            ModelState.AddModelError("", "Status gick inte att ändra, felaktigt byte!");
            var vmodel = OrderViewModel.Create(order);
            return View("OrderDetails", vmodel);
        }

        bool isChanged = order.ChangeStatus(Logic.Enums.OrderStatus.Cancelled);

        if (!isChanged)
        {
            ModelState.AddModelError("", "Status gick inte att ändra, felaktigt byte!");
            var vmodel = OrderViewModel.Create(order);
            return View("OrderDetails", vmodel);
        }

        order = await _service.UpdateStatus(order);

        var viewModel = OrderViewModel.Create(order);

        return View("OrderDetails", viewModel);

    }
}