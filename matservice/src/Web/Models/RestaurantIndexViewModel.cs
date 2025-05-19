using Logic.Entities;

namespace Web.Models;

public class RestaurantIndexViewModel
{
    public required string RestaurantIdentifier { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string PictureUri { get; set; } = "images/no-image.png";
    public List<OpeningHourViewModel> OpeningHours { get; set; } = [];
    public List<CourseViewModel> Menu { get; private set; } = [];
    public decimal DeliveryFee { get; set; }

    public static RestaurantIndexViewModel Create(Restaurant restaurant)
    {
        return new RestaurantIndexViewModel
        {
            RestaurantIdentifier = restaurant.RestaurantIdentifier,
            Name = restaurant.Name,
            Description = restaurant.Description,
            OpeningHours = restaurant.OpeningHours?.Select(OpeningHourViewModel.MapToOpeningHoursViewModel).ToList() ?? new List<OpeningHourViewModel>(),
            PictureUri = restaurant.PictureUri,
            DeliveryFee = restaurant.DeliveryFee,
            Menu = restaurant.Menu?.Select(CourseViewModel.MapToCourseViewModel).ToList() ?? new List<CourseViewModel>(),
        };
    }

}