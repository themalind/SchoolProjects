using System.Linq.Expressions;

using Infrastructure.Data;

using Logic.Entities;
using Logic.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RestaurantRepository(IDbContextFactory<FoodDbContext> context) : IRestaurantRepository
{
    private readonly IDbContextFactory<FoodDbContext> _context = context;

    public async Task<Restaurant> AddAsync(string name, List<OpeningHours> hours, string description, string pictureUri, decimal fee, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        Restaurant restaurant = new(name, hours, description, pictureUri, fee);

        context.Restaurants.Add(restaurant);

        await context.SaveChangesAsync(token);

        return restaurant;
    }

    public async Task<IReadOnlyCollection<Restaurant>> ListAsync(CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        var list = await context.Restaurants
        .AsNoTracking()
        .Include(r => r.Menu)
        .ThenInclude(m => m.Category)
        .Include(r => r.OpeningHours)
        .ToListAsync(token);

        return list;
    }

    public async Task AddCategoryAsync(Category category, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        await context.Categories.AddAsync(category);

        await context.SaveChangesAsync(token);
    }

    public async Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        return await context.Categories.ToListAsync(token);
    }

    public async Task<Restaurant?> GetByIdentifierAsync(string identifier, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        var restaurant = await context.Restaurants
            .Include(f => f.Menu)
            .ThenInclude(m => m.Category)
            .Include(oh => oh.OpeningHours)
            .Where(r => r.RestaurantIdentifier == identifier)
            .FirstOrDefaultAsync();

        return restaurant;
    }

    public async Task<FoodCourse> AddCourseToMenuAsync(string restaurantIdentifier,
        string name, string description,
        List<Category> categories, decimal unitPrice,
        string pictureuri,
        CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        FoodCourse foodCourse = new(restaurantIdentifier, name, description, categories, unitPrice, pictureuri);

        var restaurant = await context.Restaurants
            .FirstOrDefaultAsync(r => r.RestaurantIdentifier == foodCourse.RestaurantIdentifier, token);

        if (restaurant is null) { throw new ArgumentException("Resturant not found!"); }

        restaurant.Menu.Add(foodCourse);

        await context.SaveChangesAsync(token);

        return foodCourse;
    }

    public async Task<FoodCourse?> GetFoodCourseByIdentifierAsync(string identifier, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        var course = await context.FoodCourses.FirstOrDefaultAsync(f => f.FoodCourseIdentifier == identifier, token);

        return course;
    }

}