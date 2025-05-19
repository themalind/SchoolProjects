namespace Logic.Entities;

public class FoodCourse
{
    public int Id { get; private set; }
    public string FoodCourseIdentifier { get; set; }
    public string RestaurantIdentifier { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; } = 119;
    public List<Category> Category { get; private set; } = [];
    public string PictureUri { get; private set; }

#pragma warning disable CS8618
    private FoodCourse() { }

    public FoodCourse(string restaurantIdentifier,
        string name,
        string description,
        List<Category> category, decimal unitPrice,
        string pictureUri)
    {
        RestaurantIdentifier = restaurantIdentifier;
        Name = name;
        Description = description;
        Category = category;
        UnitPrice = unitPrice;
        PictureUri = pictureUri;
    }
}