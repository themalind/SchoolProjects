using System.ComponentModel.DataAnnotations;

namespace FoodAPI.EndPoints;

public class RequestRestaurant
{
    public required string Name { get; set; }
    public List<OpeningHoursDto> OpeningHours { get; set; } = [];
    public required string Description { get; set; }
    public required string PictureUri { get; set; }
    [Range(0, 1000)]
    public decimal DeliveryFee { get; set; }
}

