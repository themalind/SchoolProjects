using System.ComponentModel.DataAnnotations;

namespace Web.Models;
public class BasketItemViewModel
{
    [Required]
    public required string FoodCourseIdentifier { get; set; }

    [Required]
    public required string RestaurantIdentifier { get; set; }

    [Required]
    public string? ProductName { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Priset får inte vara negativt.")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Range(1, 10)]
    public int Quantity { get; set; }
    public string? PictureUrl { get; set; }
}