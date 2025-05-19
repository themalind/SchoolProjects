using FastEndpoints;

using Logic.Interfaces;

namespace FoodAPI.EndPoints;

public class AssignCourierToOrder : Endpoint<AssignRequest, AssignResponse>
{
    public required IOrderRepository Repo { get; set; }
    public override void Configure()
    {
        Put("/api/Order/assigncourier");

    }

    public override async Task HandleAsync(AssignRequest request, CancellationToken ct)
    {
        var order = await Repo.GetByOrderNumberAsync(request.OrderNumber, ct);

        if (order is null)
        {
            ThrowError("Order not found", statusCode: 404);
        }

        var courier = await Repo.GetCourierAsync(request.CourierId, ct);

        if (courier is null)
        {
            ThrowError("Courier not found", statusCode: 404);
        }

        if (order.Courier != null)
        {
            ThrowError($"Already assaigned to {order.Courier.Name}", statusCode: 400);
        }

        if (order.Status != Logic.Enums.OrderStatus.Confirmed)
        {
            ThrowError("Orderstatus måste vara Confirmed för att ett bud ska kunna acceptera.", statusCode: 400);
        }

        var updatedOrder = await Repo.AssignOrderToCourierAsync(order, courier, ct);

        var response = AssignResponse.CreateAsignResponse(courier, updatedOrder);

        await SendAsync(response, cancellation: ct);
    }

}