namespace FoodAPI.EndPoints;

public class AssignRequest
{
    public required int CourierId { get; set; }
    public required string OrderNumber { get; set; }
}