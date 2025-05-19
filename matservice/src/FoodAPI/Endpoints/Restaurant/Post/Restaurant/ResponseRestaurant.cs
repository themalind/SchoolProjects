using System.ComponentModel.DataAnnotations;

using Azure;

using Logic.Entities;

namespace FoodAPI.EndPoints;

public class ResponseRestaurant
{
    public required string RestaurantIdentifier { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string PictureUri { get; set; }
    public IReadOnlyCollection<OpeningHoursDto> OpeningHours { get; set; } = [];
    [Range(0, 1000)]
    public decimal DeliveryFee { get; set; }

    public static ResponseRestaurant CreateResponseRestaurant(Restaurant restaurant)
    {
        return new ResponseRestaurant
        {
            RestaurantIdentifier = restaurant.RestaurantIdentifier,
            Name = restaurant.Name,
            OpeningHours = restaurant.OpeningHours.Select(oh => new OpeningHoursDto
            {
                Day = oh.Day,
                OpenTime = oh.OpenTime,
                CloseTime = oh.CloseTime,
                KitchenCloseTime = oh.KitchenCloseTime
            }).ToList(),
            Description = restaurant.Description,
            PictureUri = restaurant.PictureUri,
            DeliveryFee = restaurant.DeliveryFee
        };
    }
}

