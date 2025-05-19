using FastEndpoints;

using FluentValidation;

namespace FoodAPI.EndPoints;

public class FoodCourseValidator : Validator<RequestFoodCourse>
{
    public FoodCourseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The foodcourse must have a name!")
            .MinimumLength(5)
            .WithMessage("Name to short!");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Describe the dish!")
            .MinimumLength(10)
            .WithMessage("Description to short!");

        RuleFor(x => x.RestaurantIdentifier)
            .NotEmpty()
            .WithMessage("A new dish must have a restaurant!")
            .MinimumLength(10)
            .WithMessage("Identifier to short!");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("No negative numbers!");
    }
}