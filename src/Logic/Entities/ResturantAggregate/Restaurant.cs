namespace Logic.Entities;

public class Restaurant
{
    public int Id { get; init; }
    public string RestaurantIdentifier { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PictureUri { get; set; }
    public List<OpeningHours> OpeningHours { get; set; } = [];
    public List<FoodCourse> Menu { get; private set; } = [];
    public decimal DeliveryFee { get; set; }

#pragma warning disable CS8618
    private Restaurant()
    { }

    public Restaurant(string name, List<OpeningHours> openingHours, string description, string pictureUri, decimal deliveryFee)
    {
        Name = name;
        OpeningHours = openingHours;
        Description = description;
        PictureUri = pictureUri;
        DeliveryFee = deliveryFee;
    }

}