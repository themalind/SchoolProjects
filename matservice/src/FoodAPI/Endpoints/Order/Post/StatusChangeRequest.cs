namespace FoodAPI.EndPoints;

public class StatusChangeRequest
{
    public required string OrderStatus { get; set; }
    public required string OrderNumber { get; set; }
}