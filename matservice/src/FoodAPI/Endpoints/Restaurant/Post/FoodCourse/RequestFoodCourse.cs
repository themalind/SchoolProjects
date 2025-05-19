namespace FoodAPI.EndPoints;

public class RequestFoodCourse
{
    public required string RestaurantIdentifier { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<CategoryDto> Category { get; set; } = [];
    public decimal UnitPrice { get; set; }
    public required string PictureUri { get; set; }
}