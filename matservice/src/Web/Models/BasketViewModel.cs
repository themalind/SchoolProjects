namespace Web.Models;

public class BasketViewModel
{
    public List<BasketItemViewModel> Items { get; set; } = [];
    public Dictionary<string, decimal> DeliveryFee { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public CreateOrderViewModel CreateOrderData { get; set; } = new();
    public decimal Total()
    {
        TotalPrice = Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity) + DeliveryFee.Sum(fee => fee.Value), 2);

        return TotalPrice;
    }

    public decimal ServiceFee()
    {
        decimal serviceFee = TotalPrice * 0.05m;

        return serviceFee;
    }
}