using FastEndpoints;

using Logic.Enums;
using Logic.Interfaces;

namespace FoodAPI.EndPoints;

public class ChangeOrderStatus : Endpoint<StatusChangeRequest, ChangestatusResponse>
{
    public required IOrderRepository Repo { get; set; }
    public override void Configure()
    {
        Put("/api/orders/status/");
    }

    public override async Task HandleAsync(StatusChangeRequest request, CancellationToken ct)
    {
        var order = await Repo.GetByOrderNumberAsync(request.OrderNumber, ct);

        if (order is null)
        {
            ThrowError("Order not found", statusCode: 400);
        }

        if (!Enum.TryParse<OrderStatus>(request.OrderStatus, true, out var status))
        {
            ThrowError("Invalid status value", statusCode: 400);
        }

        bool isChanged = order.ChangeStatus(status);

        if (!isChanged)
        {
            ThrowError("Invalid change", statusCode: 400);
        }

        await Repo.ChangeOrderStatusAsync(order, status, ct);

        order = await Repo.GetByOrderNumberAsync(request.OrderNumber, ct);

        var response = ChangestatusResponse.CreateChangeStatusResponse(order!);


        await SendAsync(response, cancellation: ct);
    }
}
