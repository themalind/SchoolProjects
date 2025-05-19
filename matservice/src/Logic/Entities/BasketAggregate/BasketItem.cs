namespace Web.Models;

public class BasketItem
{
    public int Id { get; set; }
    public string FoodCourseIdentifier { get; set; }
    public string RestaurantIdentifier { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

#pragma warning disable CS8618    
    public BasketItem() { }
#pragma warning restore CS8618
    public BasketItem(
        string foodCourseIdentifier,
        string restaurantIdentifier,
        int quantity,
        decimal unitPrice)
    {
        FoodCourseIdentifier = foodCourseIdentifier;
        RestaurantIdentifier = restaurantIdentifier;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;

        if (Quantity > 10) Quantity = 10;
    }

}