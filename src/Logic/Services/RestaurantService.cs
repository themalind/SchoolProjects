using Logic.Entities;
using Logic.Interfaces;
namespace Logic.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _repository;

    public RestaurantService(IRestaurantRepository repository)
    {
        _repository = repository;
    }

    public async Task<Restaurant> AddAsync(Restaurant restaurant, CancellationToken token = default)
    {
        await _repository.AddAsync(restaurant.Name, restaurant.OpeningHours, restaurant.Description, restaurant.PictureUri, restaurant.DeliveryFee, token);

        return restaurant;
    }

    public async Task<IReadOnlyCollection<Restaurant>> ListAsync(CancellationToken token = default)
    {
        var restaurants = await _repository.ListAsync(token);

        return restaurants;
    }

    public async Task<Category> AddCategoryAsync(Category categoryDto, CancellationToken token = default)
    {
        Category category = new(categoryDto.Name);

        await _repository.AddCategoryAsync(category, token);

        return category;
    }

    public async Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CancellationToken token = default)
    {
        var list = await _repository.GetCategoriesAsync(token);

        return list;
    }
    public async Task<Restaurant?> GetByIdentifierAsync(string identifier, CancellationToken token = default)
    {
        var restaurant = await _repository.GetByIdentifierAsync(identifier, token);

        return restaurant;
    }

    public async Task<FoodCourse?> GetFoodCourseByIdentifierAsync(string identifier, CancellationToken token = default)
    {
        var course = await _repository.GetFoodCourseByIdentifierAsync(identifier, token);

        return course;
    }

}