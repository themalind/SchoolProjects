using System.ComponentModel.DataAnnotations;

namespace Web.Models;
public class AddToBasketViewModel
{
    [Required]
    public string CourseIdentifier { get; set; } = "";

    [Range(1, 10, ErrorMessage = "Invalid quantity. Must be larger than 0 and max 10, the food is not infinite.")]
    public int Quantity { get; set; }

}