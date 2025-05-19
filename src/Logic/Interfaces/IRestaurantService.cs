using Logic.Entities;

namespace Logic.Interfaces;

public interface IRestaurantService
{
    Task<Restaurant?> GetByIdentifierAsync(string identifier, CancellationToken token = default);
    Task<IReadOnlyCollection<Restaurant>> ListAsync(CancellationToken token = default);
    Task<Restaurant> AddAsync(Restaurant restaurant, CancellationToken token = default);
    Task<Category> AddCategoryAsync(Category category, CancellationToken token = default);
    Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CancellationToken token = default);
    Task<FoodCourse?> GetFoodCourseByIdentifierAsync(string identifier, CancellationToken token = default);
}