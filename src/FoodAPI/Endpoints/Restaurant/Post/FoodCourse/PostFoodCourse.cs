using FastEndpoints;

using Logic.Entities;
using Logic.Interfaces;

namespace FoodAPI.EndPoints;

public class PostFoodCourse : Endpoint<RequestFoodCourse, ResponseFoodcourse>
{
    public required IRestaurantRepository Repo { get; set; }
    public override void Configure()
    {
        Post("/api/restaurant/foodcourse");

    }

    public override async Task HandleAsync(RequestFoodCourse request, CancellationToken ct)
    {
        var categories = await Repo.GetCategoriesAsync(ct);
        var categoryList = new List<Category>();

        foreach (var requestCategory in request.Category)
        {
            var existingCategory = categories.FirstOrDefault(c => c.Name.ToLower() == requestCategory.Name.ToLower()); //kolla tolower

            if (existingCategory != null && !categoryList.Any(c => c.Name == requestCategory.Name))
            {
                categoryList.Add(existingCategory);
            }
            else if (!categoryList.Any(c => c.Name == requestCategory.Name))
            {
                Category fromDto = new(requestCategory.Name);
                await Repo.AddCategoryAsync(fromDto, ct);

                categoryList.Add(fromDto);
            }
        }

        var restaurant = await Repo.GetByIdentifierAsync(request.RestaurantIdentifier, ct);

        if (restaurant is null)
        {
            ThrowError("Restaurant not found", statusCode: 400);
        }

        var foodCourse = await Repo.AddCourseToMenuAsync(request.RestaurantIdentifier,
            request.Name, request.Description, categoryList, request.UnitPrice, request.PictureUri, ct);

        var response = ResponseFoodcourse.CreateResponseFoodCourse(foodCourse);

        await SendAsync(response, cancellation: ct);
    }


}