using FastEndpoints;

using FluentValidation;

namespace FoodAPI.EndPoints;
public class RestaurantValidator : Validator<RequestRestaurant>
{
    public RestaurantValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The restaurant MUST have a name!")
            .MinimumLength(1)
            .WithMessage("Name to short!");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Describe the restaurant!")
            .MinimumLength(10)
            .WithMessage("Description to short!");

        RuleFor(x => x.DeliveryFee)
           .GreaterThan(-1)
           .WithMessage("A fee is no fee if it's below 0!");

    }
}