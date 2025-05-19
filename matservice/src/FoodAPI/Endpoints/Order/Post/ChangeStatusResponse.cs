using Logic.Entities;

namespace FoodAPI.EndPoints;

public class ChangestatusResponse
{
    public required OrderDto Response { get; set; }

    public static ChangestatusResponse CreateChangeStatusResponse(Order order)
    {
        return new ChangestatusResponse
        {
            Response = new OrderDto()
            {
                OrderNumber = order!.OrderNumber,
                OrderDate = order.OrderDate,
                Customer = new CustomerDto() { FullName = order.Customer.FullName, PhoneNumber = order.Customer.PhoneNumber },
                Address = new AddressDto() { Street = order.Address.Street, ZipCode = order.Address.ZipCode, City = order.Address.City },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto()
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