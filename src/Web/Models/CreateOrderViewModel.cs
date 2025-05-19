using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class CreateOrderViewModel // formuläret
{
    [Required]
    [MinLength(2), MaxLength(20)]
    public string? FullName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Mobile no. is required")]
    [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
    public string? PhoneNumber { get; set; }

    [Required]
    public string? StreetAddress { get; set; }

    [Required]
    [MaxLength(6)]
    public string? ZipCode { get; set; }

    [Required]
    public string? City { get; set; }


}