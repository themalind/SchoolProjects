
using Logic.Entities;

namespace Web.Models;
public class CourseViewModel
{
    public int Id { get; set; }
    public string FoodCourseIdentifier { get; set; } = "";
    public string RestaurantIdentifier { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string PictureUri { get; set; } = "";
    public List<string> Categories { get; set; } = [];
    public bool IsTheSameRestaurant { get; set; }

    public AddToBasketViewModel AddToBasketViewModel { get; set; } = new() { Quantity = 1 };

    public void FillCourseViewModel(FoodCourse course)
    {
        FoodCourseIdentifier = course.FoodCourseIdentifier;
        RestaurantIdentifier = course.RestaurantIdentifier;
        Name = course.Name;
        Description = course.Description;
        Price = course.UnitPrice;
        PictureUri = course.PictureUri;
        Categories = course?.Category.Select(c => c.Name).ToList() ?? new List<string>();
    }

    public static CourseViewModel MapToCourseViewModel(FoodCourse course)
    {
        return new CourseViewModel
        {
            FoodCourseIdentifier = course.FoodCourseIdentifier,
            RestaurantIdentifier = course.RestaurantIdentifier,
            Name = course.Name,
            Description = course.Description,
            Price = course.UnitPrice,
            PictureUri = course.PictureUri,
            Categories = course?.Category.Select(c => c.Name).ToList() ?? new List<string>()
        };
    }
}

