using FastEndpoints;

using Logic.Interfaces;

namespace FoodAPI.EndPoints;

public class GetRestaurantsEndpoint : EndpointWithoutRequest<ResponseRestaurants>
{
    public required IRestaurantRepository Repo { get; init; }

    public GetRestaurantsEndpoint(IRestaurantRepository restaurantRepository)
    {
        this.Repo = restaurantRepository;
    }

    public override void Configure()
    {
        Get("/api/restaurants");

    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var list = await Repo.ListAsync(ct);

        if (list is null || !list.Any()) { ThrowError("No restaurants found", statusCode: 400); }

        var response = ResponseRestaurants.CreateRestaurantResponse(list);

        await SendAsync(response, cancellation: ct);
    }
}
