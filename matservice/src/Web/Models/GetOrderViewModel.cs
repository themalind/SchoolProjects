using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class GetOrderViewModel
{
    [Required]
    public required string OrderNumber { get; set; }

    [Required(ErrorMessage = "Mobile no. is required")]
    [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
    public required string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(2), MaxLength(30)]
    public required string FullName { get; set; }
}