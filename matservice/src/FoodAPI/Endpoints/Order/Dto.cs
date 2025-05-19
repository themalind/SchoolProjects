using Logic.Entities;

namespace FoodAPI.EndPoints;
public class AddressDto
{
    public required string Street { get; set; }
    public required string ZipCode { get; set; }
    public required string City { get; set; }
}

public class CustomerDto
{
    public required string FullName { get; set; }
    public required string PhoneNumber { get; set; }
}

public class OrderDto
{
    public required string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public required AddressDto Address { get; set; }
    public required CustomerDto Customer { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public required string OrderStatus { get; set; }
    public decimal TotalPrice { get; set; }
    public TimeSpan? WaitTime { get; set; }
    public Courier? Courier { get; set; }
}

public class OrderItemDto
{
    public required string RestaurantIdentifier { get; set; }
    public required string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}