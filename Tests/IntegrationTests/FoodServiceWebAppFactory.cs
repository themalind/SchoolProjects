using Infrastructure.Data;

using Logic.Entities;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class FoodServiceWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //Ta bort alla LoanDbContextrelaterade konfigurationer
            services.RemoveAll<IDbContextOptionsConfiguration<FoodDbContext>>();

            // Registrera dbcontext med connectionstring till en testdatabas.
            services.AddDbContextFactory<FoodDbContext>(options => options.UseSqlServer(""));

            //Skapa databasen --> concurrencyproblem vid flera som arbetar med samma projekt?
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FoodDbContext>();
            dbContext.Database.EnsureCreated();

            //Seeda databasen

            // Skapa kunder
            var customerOne = new Customer("John Doe", "0707123456", "hejhej@test.com");
            dbContext.Customers.Add(customerOne);
            var customerTwo = new Customer("John Doe", "0707123765", "hejhej@test.com");
            dbContext.Customers.Add(customerTwo);
            var customerThree = new Customer("John Doe", "0707123765", "hejhej@test.com");
            dbContext.Customers.Add(customerThree);

            // Skapa Adresser
            var adressOne = new Address("Stigen 29", "Borås", "50465");
            dbContext.Addresses.Add(adressOne);
            var adressTwo = new Address("Stenvägen 9", "Borås", "50405");
            dbContext.Addresses.Add(adressTwo);
            var adressThree = new Address("trädgatan", "Borås", "50434");
            dbContext.Addresses.Add(adressThree);

            // Skapa öppettider
            var openingHours = new List<OpeningHours> {
                new OpeningHours (Logic.Enums.WeekDay.Monday, new TimeOnly(8,0), new TimeOnly(16,0), new TimeOnly(15,0) ),
                new OpeningHours (Logic.Enums.WeekDay.Tuesday, new TimeOnly(8,0), new TimeOnly(16,0), new TimeOnly(15,0) ),
                new OpeningHours (Logic.Enums.WeekDay.Wednesday, new TimeOnly(8,0), new TimeOnly(16,0), new TimeOnly(15,0) ),
                new OpeningHours (Logic.Enums.WeekDay.Thursday, new TimeOnly(8,0), new TimeOnly(16,0), new TimeOnly(15,0) ),
                new OpeningHours (Logic.Enums.WeekDay.Friday, new TimeOnly(8,0), new TimeOnly(16,0), new TimeOnly(15,0) )
            };

            dbContext.OpeningHours.AddRange(openingHours);

            // skapa Restaurang
            dbContext.Restaurants.Add(new Restaurant("Laxstugan", openingHours, "Ät inte en tugga till, det är inte lax det är fax!", "images/picture.png", 79.95m));

            // Skapa Kategorier
            var categories = new List<Category>{
                new Category("Fiskigt"),
                new Category("Delikatess"),
                new Category("Veckans Fångst")
            };

            dbContext.Categories.AddRange(categories);

            // Skapa Meny till restarangen
            dbContext.FoodCourses.Add(new FoodCourse("REST-0001000", "Laxtartar", "Smarrigt värre", categories, 149.95m, "images/picture.png"));
            dbContext.FoodCourses.Add(new FoodCourse("REST-0001000", "Gravad lax", "Gravad på eget vis, serveras med hovmästarsås", categories, 129m, "images/picture.png"));
            dbContext.FoodCourses.Add(new FoodCourse("REST-0001000", "Laxpudding", "Pudding med lax och dill", categories, 149.95m, "images/picture.png"));
            dbContext.FoodCourses.Add(new FoodCourse("REST-0001000", "Laxkaka", "Potatiskaka med lax, spenat, lök och dill", categories, 169.95m, "images/picture.png"));
            dbContext.FoodCourses.Add(new FoodCourse("REST-0001000", "Varmrökt lax", "Lyx! lyx! ", categories, 149.95m, "images/picture.png"));

            // skapa bud
            dbContext.Couriers.Add(new Courier("Anders Andersson"));
            dbContext.Couriers.Add(new Courier("Klara Klarasson"));

            // Skapa order items
            var itemsOne = new List<OrderItem>{
                new OrderItem("Laxtartar", 2, 149.95m, "REST-0001000", "FOOD-0001000" ),
                new OrderItem("Laxpudding", 1, 149.95m, "REST-0001000", "FOOD-0001005")
            };

            var itemsTwo = new List<OrderItem>{
                new OrderItem("Gravad lax", 1, 129m, "REST-0001000", "FOOD-0001010" ),
                new OrderItem("Laxkaka", 1, 169.95m, "REST-0001000", "FOOD-0001015")
            };

            var itemsThree = new List<OrderItem>{
                new OrderItem("Varmrökt lax", 1,149.95m, "REST-0001000", "FOOD-0001020")
            };

            // Skapa ordrar
            dbContext.Orders.Add(new Order(customerOne, adressOne, itemsOne, 79, Logic.Enums.OrderStatus.Delivered));
            dbContext.Orders.Add(new Order(customerTwo, adressTwo, itemsTwo, 79, Logic.Enums.OrderStatus.Delivered));
            dbContext.Orders.Add(new Order(customerThree, adressThree, itemsThree, 79, Logic.Enums.OrderStatus.Delivered));

            dbContext.SaveChanges();
        });
    }
}

public class WebAppFactoryFixture : IDisposable
{
    public FoodServiceWebAppFactory Factory { get; private set; }

    public WebAppFactoryFixture()
    {
        Factory = new FoodServiceWebAppFactory();
    }

    public void Dispose()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FoodDbContext>();

        // När testerna är körda, tas testdatabasen bort.
        dbContext.Database.EnsureDeleted();

        Factory.Dispose();
        Factory = null!;
    }
}