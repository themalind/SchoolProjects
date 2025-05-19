using System.ComponentModel.DataAnnotations;

using Logic.Enums;
namespace FoodAPI.EndPoints;

public class RestaurantDto
{
    public string RestaurantIdentifier { get; set; } = "";
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public string PictureUri { get; set; } = "images/no-image.png";
    public List<OpeningHoursDto> OpeningHours { get; set; } = [];
    public List<FoodCourseDto> Menu { get; set; } = [];
    [Range(0, 1000)]
    public decimal DeliveryFee { get; set; }
}

public class OpeningHoursDto
{
    public int Id { get; set; }
    public WeekDay Day { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public TimeOnly KitchenCloseTime { get; set; }
}

public class FoodCourseDto
{
    public string FoodCourseIdentifier { get; set; } = "";
    public required string RestaurantIdentifier { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<CategoryDto> Category { get; set; } = [];
    public decimal UnitPrice { get; set; }
    public required string PictureUri { get; set; }
}

public class CategoryDto
{
    public required string Name { get; set; }
}