using Logic.Entities;

using Microsoft.EntityFrameworkCore;

using Web.Models;

namespace Infrastructure.Data;

public class FoodDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<OpeningHours> OpeningHours { get; set; }
    public DbSet<FoodCourse> FoodCourses { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Courier> Couriers { get; set; }

    public FoodDbContext(DbContextOptions<FoodDbContext> options)
       : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FoodCourse>()
            .HasMany(f => f.Category)
            .WithMany(); // Category har ingen navigationsegenskap, men EF vet att det är så och kommmer skapa åt mig 

        modelBuilder.HasSequence<int>("OrderNumbers") // https://learn.microsoft.com/en-us/ef/core/modeling/sequences
        .StartsAt(1000) // sekvensen startar på 1000
        .IncrementsBy(5); // och ökar med 5

        modelBuilder.Entity<Order>() // Detta är för att skapa unika ordernummer.
       .Property(o => o.OrderNumber)
       .HasDefaultValueSql("CONCAT('ORD-', FORMAT(NEXT VALUE FOR OrderNumbers, '0000000'))");
        // Den lägger till "ORD" och sen nollor så att det blir totalt 7 siffror.

        modelBuilder.HasSequence<int>("Restaurant")
        .StartsAt(1000)
        .IncrementsBy(5);

        modelBuilder.Entity<Restaurant>()
       .Property(r => r.RestaurantIdentifier)
       .HasDefaultValueSql("CONCAT('REST-', FORMAT(NEXT VALUE FOR OrderNumbers, '0000000'))");

        modelBuilder.HasSequence<int>("FoodCourseIdentifier")
        .StartsAt(1000)
        .IncrementsBy(5);

        modelBuilder.Entity<FoodCourse>()
       .Property(f => f.FoodCourseIdentifier)
       .HasDefaultValueSql("CONCAT('FOOD-', FORMAT(NEXT VALUE FOR OrderNumbers, '0000000'))");

    }
}