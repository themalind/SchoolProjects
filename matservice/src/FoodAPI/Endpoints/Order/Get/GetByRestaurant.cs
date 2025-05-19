using FastEndpoints;

using Logic.Interfaces;

namespace FoodAPI.EndPoints;

public class GetByRestaurants : EndpointWithoutRequest<OrdersResponse>
{
    public required IOrderRepository Repo { get; set; }
    public override void Configure()
    {
        Get("/api/Orders/{RestaurantIdentifier}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var restaurantIdentifier = Route<string>("RestaurantIdentifier");

        if (string.IsNullOrEmpty(restaurantIdentifier))
        {
            ThrowError("Restaurant identifier is required", statusCode: 400);
        }

        var orders = await Repo.ListAsync(o =>
        o.OrderItems.Any(oi => oi.RestaurantIdentifier == restaurantIdentifier), ct);

        if (orders is null || !orders.Any())
        {
            ThrowError("No orders found for the given restaurant", statusCode: 404);
        }

        var response = OrdersResponse.CreateOrdersResponse(orders);

        await SendAsync(response, cancellation: ct);
    }
}