namespace Web.Models;

public class Basket
{
    public int Id { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; } = [];
    public decimal Deliveryfee { get; set; }

#pragma warning disable CS8618
    public Basket() { }
#pragma warning restore CS8618

    public Basket(IEnumerable<BasketItem> basketItems)
    {
        BasketItems = [.. basketItems]; // Här tillverkas en collection
    }

    public void AddBasketItem(BasketItem basketItem)
    {
        if (!BasketItems.Any(oi => oi.FoodCourseIdentifier == basketItem.FoodCourseIdentifier))
        {
            BasketItems.Add(basketItem);
            return;
        }

        var item = BasketItems.First(oi => oi.FoodCourseIdentifier == basketItem.FoodCourseIdentifier);

        item.AddQuantity(basketItem.Quantity);
    }

    public void RemoveFromBasket(BasketItem course)
    {
        BasketItems.Remove(course);
    }

    public void AddDeliveryFee(decimal fee)
    {
        Deliveryfee = fee;
    }

    public void ClearBasket()
    {
        BasketItems.Clear();
    }
}