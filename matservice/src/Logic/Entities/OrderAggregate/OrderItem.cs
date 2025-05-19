namespace Logic.Entities;

public class OrderItem
{
    public int Id { get; private set; }
    public string FoodCourseName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string RestaurantIdentifier { get; set; }
    public string FoodCourseIdentifier { get; set; }

#pragma warning disable CS8618
    private OrderItem() { }
#pragma warning restore CS8618

    public OrderItem(string foodCourseName,
        int quantity,
        decimal unitPrice, string restaurantIdentifier, string foodCourseIdentifier)
    {
        FoodCourseName = foodCourseName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        RestaurantIdentifier = restaurantIdentifier;
        FoodCourseIdentifier = foodCourseIdentifier;
    }
}