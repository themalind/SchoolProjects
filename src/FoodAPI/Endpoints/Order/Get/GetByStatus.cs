using FastEndpoints;

using Logic.Enums;
using Logic.Interfaces;

namespace FoodAPI.EndPoints;

public class GetByStatus : EndpointWithoutRequest<OrdersResponse>
{
    public required IOrderRepository Repo { get; set; }
    public override void Configure()
    {
        Get("/api/orders/Status/{OrderStatus}");

    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var orderStatus = Route<string>("OrderStatus");

        if (!Enum.TryParse<OrderStatus>(orderStatus, true, out var status))
        {
            ThrowError("Invalid status", statusCode: 400);
        }

        var list = await Repo.ListAsync(o => o.Status == status, ct);

        if (list is null || !list.Any())
        {
            ThrowError("Status not found", statusCode: 400);
        }

        var response = OrdersResponse.CreateOrdersResponse(list);

        await SendAsync(response, cancellation: ct);
    }

}