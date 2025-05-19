using Logic.Entities;

namespace FoodAPI.EndPoints;
public class ResponseFoodcourse
{
    public required string FoodCourseIdentifier { get; set; }
    public required string RestaurantIdentifier { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<CategoryDto> Category { get; set; } = [];
    public decimal UnitPrice { get; set; }
    public required string PictureUri { get; set; }

    public static ResponseFoodcourse CreateResponseFoodCourse(FoodCourse foodCourse)
    {
        return new ResponseFoodcourse
        {
            FoodCourseIdentifier = foodCourse.FoodCourseIdentifier,
            RestaurantIdentifier = foodCourse.RestaurantIdentifier,
            Name = foodCourse.Name,
            Description = foodCourse.Description,
            Category = foodCourse.Category.Select(c => new CategoryDto { Name = c.Name }).ToList(),
            UnitPrice = foodCourse.UnitPrice,
            PictureUri = foodCourse.PictureUri
        };
    }


}