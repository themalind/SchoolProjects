using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class RemoveBasketItemViewModel
{
    [Required]
    public required string FoodCourseIdentifier { get; set; }
}