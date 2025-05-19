using Logic.Enums;

namespace Logic.Entities;

public class Order
{
    public int Id { get; private set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; init; }
    public Address Address { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
    public OrderStatus Status { get; set; } = OrderStatus.Received;
    public Courier? Courier { get; set; }
    public TimeSpan? WaitTime { get; set; }
    public decimal Deliveryfee { get; set; }
    const decimal ServiceFee = 0.05m;


#pragma warning disable CS8618
    private Order() { }

    public Order(Customer customer,
        Address address,
        List<OrderItem> orderItems, decimal deliveryfee,
        OrderStatus status = 0) // 0 = Received
    {
        Customer = customer;
        OrderDate = DateTime.UtcNow.ToLocalTime();
        Address = address;
        OrderItems = orderItems;
        Deliveryfee = deliveryfee;
        Status = status;
        WaitTime = SetWaitTime();

    }
#pragma warning restore CS8618

    public decimal GetTotalPrice()
    {
        decimal itemsSum = OrderItems.Sum(item => item.UnitPrice * item.Quantity);
        decimal service = (itemsSum + Deliveryfee) * ServiceFee;
        decimal totalSum = itemsSum + Deliveryfee + service;

        return Math.Round(totalSum, 2);
    }

    private TimeSpan SetWaitTime()
    {
        Random random = new Random();
        int minuter = random.Next(30, 91);
        return TimeSpan.FromMinutes(minuter);
    }

    public bool ChangeStatus(OrderStatus newStatus)
    {
        if (!CanTransition(newStatus))
        {
            return false;
        }

        Status = newStatus;

        return true;
    }

    private bool CanTransition(OrderStatus newStatus)
    {
        return (Status, newStatus) switch
        {
            (OrderStatus.Received, OrderStatus.Cancelled) => true,
            (OrderStatus.Received, OrderStatus.Confirmed) => true,
            (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
            (OrderStatus.Confirmed, OrderStatus.CourierAccepted) when Courier != null => true, // Case guards
            (OrderStatus.CourierAccepted, OrderStatus.Cancelled) => true,
            (OrderStatus.CourierAccepted, OrderStatus.Preparing) when Courier != null => true,
            (OrderStatus.Preparing, OrderStatus.Cancelled) => true,
            (OrderStatus.Preparing, OrderStatus.ReadyForPickup) when Courier != null => true,
            (OrderStatus.ReadyForPickup, OrderStatus.Cancelled) => true,
            (OrderStatus.ReadyForPickup, OrderStatus.InTransit) when Courier != null => true,
            (OrderStatus.InTransit, OrderStatus.Cancelled) => true,
            (OrderStatus.InTransit, OrderStatus.Delivered) when Courier != null => true,
            (OrderStatus.Delivered, OrderStatus.Cancelled) => true,
            _ => false,
        };
    }

}