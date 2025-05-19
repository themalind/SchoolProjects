using System.Linq.Expressions;

using Logic.Entities;

namespace Logic.Interfaces;

public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdentifierAsync(string identifier, CancellationToken token = default);
    Task<IReadOnlyCollection<Restaurant>> ListAsync(CancellationToken token = default);
    Task<Restaurant> AddAsync(string name, List<OpeningHours> hours, string description, string pictureUri, decimal fee, CancellationToken token = default);
    Task<FoodCourse> AddCourseToMenuAsync(string restaurantIdentifier,
        string name, string description, List<Category> categories, decimal unitPrice, string pictureuri, CancellationToken token = default);
    Task AddCategoryAsync(Category category, CancellationToken token = default);
    Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CancellationToken token = default);
    Task<FoodCourse?> GetFoodCourseByIdentifierAsync(string identifier, CancellationToken token = default);
}