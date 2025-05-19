using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
        }

        services.AddDbContextFactory<FoodDbContext>(o => { o.UseSqlServer(connectionString); });

        // Lägga till repositories här?
    }
}

// Extension-metoder används för att utöka IServiceCollection, vilket gör konfigurationen modulär och enkel att hantera.
// "this" i metodsignaturen betyder att detta är en extension method, 
//  vilket gör att vi kan anropa den på en IServiceCollection-instans som om den var en inbyggd metod.

// AddDbContextFactory<FoodDbContext>: Registrerar en DbContext Factory, 
// vilket innebär att FoodDbContext kan skapas vid behov istället för att ha en enda instans per Scoped request.
// Detta ger förbättrad prestanda vid scenarion där DbContext används tillfälligt, t.ex. i bakgrundstjänster.
// Undviker problem med trådhantering i asynkrona operationer.