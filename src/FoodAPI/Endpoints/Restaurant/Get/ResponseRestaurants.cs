using Logic.Entities;

namespace FoodAPI.EndPoints;

public class ResponseRestaurants
{
    public required IReadOnlyCollection<RestaurantDto> Restaurants { get; set; }

    public static ResponseRestaurants CreateRestaurantResponse(IReadOnlyCollection<Restaurant> list)
    {
        return new ResponseRestaurants
        {
            Restaurants = list.Select(r => new RestaurantDto
            {
                RestaurantIdentifier = r.RestaurantIdentifier,
                Name = r.Name,
                Description = r.Description,
                PictureUri = r.PictureUri,
                DeliveryFee = r.DeliveryFee,
                OpeningHours = r.OpeningHours.Select(oh => new OpeningHoursDto
                {
                    Day = oh.Day,
                    OpenTime = oh.OpenTime,
                    CloseTime = oh.CloseTime,
                    KitchenCloseTime = oh.KitchenCloseTime

                }).ToList(),

                Menu = r.Menu.Select(m => new FoodCourseDto
                {
                    FoodCourseIdentifier = m.FoodCourseIdentifier,
                    RestaurantIdentifier = m.RestaurantIdentifier,
                    Name = m.Name,
                    Description = m.Description,
                    Category = m.Category.Select(c => new CategoryDto
                    {
                        Name = c.Name
                    }).ToList(),
                    UnitPrice = m.UnitPrice,
                    PictureUri = m.PictureUri

                }).ToList()

            }).ToList()
        };
    }
}

