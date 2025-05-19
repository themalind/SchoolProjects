using FastEndpoints;

using Logic.Enums;
using Logic.Interfaces;

namespace FoodAPI.EndPoints;

public class GetOrdersFromRestaurantByStatus : EndpointWithoutRequest<OrdersResponse>
{
    public required IOrderRepository OrderRepository { get; set; }
    public required IRestaurantRepository RestaurantRepository { get; set; }
    public override void Configure()
    {
        Get("/api/orders/{Restaurantidentifier}/Status/{Orderstatus}");
    }

    public override async Task<Task> HandleAsync(CancellationToken token)
    {
        var restaurantIdentifier = Route<string>("RestaurantIdentifier");

        if (string.IsNullOrEmpty(restaurantIdentifier))
        {
            ThrowError("Restaurant not found", statusCode: 404);
        }

        var orderStatus = Route<string>("OrderStatus");

        var restaurant = await RestaurantRepository.GetByIdentifierAsync(restaurantIdentifier);

        if (restaurant is null)
        {
            ThrowError("Restaurant not found", statusCode: 404);
        }

        if (!Enum.TryParse<OrderStatus>(orderStatus, true, out var status))
        {
            ThrowError("Invalid status value", statusCode: 400);
        }

        var orders = await OrderRepository.ListAsync(o =>
           o.Status == status);

        var filteredOrders = orders.Where(order =>
            order.OrderItems.Any(oi => oi.RestaurantIdentifier == restaurantIdentifier)
        ).ToList();

        var response = OrdersResponse.CreateOrdersResponse(filteredOrders);

        return SendAsync(response, cancellation: token);
    }

}

/*
select distinct Orders.ordernumber, orders.OrderDate, orders.[status], Restaurants.Name from Orders
inner join OrderItems on OrderItems.OrderId = orders.Id
inner join Restaurants on Restaurants.RestaurantIdentifier = OrderItems.RestaurantIdentifier 
where 
OrderItems.RestaurantIdentifier = 'REST-0001010'and 
status = 1
*/