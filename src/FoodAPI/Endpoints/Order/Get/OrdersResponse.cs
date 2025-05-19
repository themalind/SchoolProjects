using Logic.Entities;
namespace FoodAPI.EndPoints;
public class OrdersResponse
{
    public required IReadOnlyCollection<OrderDto> Orders { get; set; }

    public static OrdersResponse CreateOrdersResponse(IReadOnlyCollection<Order> orders)
    {
        return new OrdersResponse
        {
            Orders = orders.Select(o => new OrderDto
            {
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                Customer = new CustomerDto() { FullName = o.Customer.FullName, PhoneNumber = o.Customer.PhoneNumber },
                Address = new AddressDto() { Street = o.Address.Street, ZipCode = o.Address.ZipCode, City = o.Address.City },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto()
                {
                    RestaurantIdentifier = oi.RestaurantIdentifier,
                    ProductName = oi.FoodCourseName,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList(),
                OrderStatus = o.Status.ToString(),
                TotalPrice = o.GetTotalPrice(),
                WaitTime = o.WaitTime,
                Courier = o.Courier
            }).ToList()
        };
    }
}