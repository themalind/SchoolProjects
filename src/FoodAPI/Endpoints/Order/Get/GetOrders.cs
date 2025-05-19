using FastEndpoints;

using Logic.Interfaces;

using Microsoft.IdentityModel.Tokens;

namespace FoodAPI.EndPoints;

public class GetOrdersEndpoint : EndpointWithoutRequest<OrdersResponse>
{
    public required IOrderRepository Repo { get; set; }
    public override void Configure()
    {
        Get("/api/orders");

    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var list = await Repo.ListAsync(ct);

        if (list is null || !list.Any())
        {
            ThrowError("No orders found", statusCode: 400);
        }

        var response = OrdersResponse.CreateOrdersResponse(list);

        await SendAsync(response, cancellation: ct);
    }
}