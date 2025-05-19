using Logic.Entities;

namespace FoodAPI.EndPoints;

public class AssignResponse
{
    public required Courier Courier { get; set; }
    public required OrderDto Order { get; set; }

    public static AssignResponse CreateAsignResponse(Courier courier, Order order)
    {
        return new AssignResponse
        {
            Courier = courier,
            Order = new OrderDto
            {
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                Customer = new CustomerDto
                {
                    FullName = order.Customer.FullName,
                    PhoneNumber = order.Customer.PhoneNumber
                },
                Address = new AddressDto
                {
                    Street = order.Address.Street,
                    ZipCode = order.Address.ZipCode,
                    City = order.Address.ZipCode
                },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    RestaurantIdentifier = oi.RestaurantIdentifier,
                    ProductName = oi.FoodCourseName,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity

                }).ToList(),
                OrderStatus = order.Status.ToString(),
                TotalPrice = order.GetTotalPrice(),
                WaitTime = order.WaitTime,
                Courier = order.Courier
            }
        };
    }

}