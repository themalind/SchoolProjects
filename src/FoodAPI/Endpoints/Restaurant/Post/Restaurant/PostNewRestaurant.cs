using FastEndpoints;

using Logic.Interfaces;
using Logic.Entities;

namespace FoodAPI.EndPoints;

public class PostNewRestaurant : Endpoint<RequestRestaurant, ResponseRestaurant>
{
    public required IRestaurantRepository Repo { get; set; }

    public override void Configure()
    {
        Post("/api/restaurants");

    }

    public override async Task HandleAsync(RequestRestaurant request, CancellationToken ct)
    {
        List<OpeningHours> openingHours = request.OpeningHours.Select(oh => new OpeningHours(
             oh.Day,
             oh.OpenTime,
             oh.CloseTime,
             oh.KitchenCloseTime)).ToList();

        var restaurant = await Repo.AddAsync(request.Name, openingHours, request.Description, request.PictureUri, request.DeliveryFee, ct);

        var response = ResponseRestaurant.CreateResponseRestaurant(restaurant);

        await SendAsync(response, cancellation: ct);
    }

}

