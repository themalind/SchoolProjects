using Logic.Entities;

namespace Web.Models;
public class OrderViewModel
{
    private const decimal ServiceFeePercent = 0.05m;
    public required string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public required string CustomerFullName { get; set; }
    public required string Address { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    public decimal TotalPrice => Items.Sum(item => item.Quantity * item.UnitPrice) * (1 + ServiceFeePercent) + DeliveryFee;
    public decimal ServiceFee => Items.Sum(item => item.Quantity * item.UnitPrice) * ServiceFeePercent;
    public decimal DeliveryFee { get; set; }
    public string OrderStatus { get; set; } = "Recieved";
    public string? Courier { get; set; }
    public TimeSpan? EstimatedDeliveryTime { get; set; }

    public static OrderViewModel Create(Order order)
    {
        return new OrderViewModel
        {
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            CustomerFullName = order.Customer.FullName,
            Address = order.Address.Street,
            Email = order.Customer.Email,
            Phone = order.Customer.PhoneNumber,
            Items = order.OrderItems.Select(oi => new OrderItemViewModel
            {
                ProductName = oi.FoodCourseName,
                RestaurantIdentifier = oi.RestaurantIdentifier,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice

            }).ToList(),
            DeliveryFee = order.Deliveryfee,
            OrderStatus = order.Status.ToString(),
            EstimatedDeliveryTime = order.WaitTime,
            Courier = order.Courier?.Name ?? "Inget bud har accepterat"
        };
    }

}