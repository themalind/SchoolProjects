using System.ComponentModel.DataAnnotations;

public class OrderItemViewModel
{
    [Required]
    public required string ProductName { get; set; }

    [Required]
    public required string RestaurantIdentifier { get; set; }

    [Required]
    [Range(1, 10)]
    public int Quantity { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Priset får inte vara negativt.")]
    public decimal UnitPrice { get; set; }

}