using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Logic.Interfaces;
using Infrastructure.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication;
using NSwag;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFastEndpoints();

        builder.Services.AddAuthorization()
                        .AddAuthentication(ApikeyAuth.SchemeName) // ApiKey = Schemaname
                        .AddScheme<AuthenticationSchemeOptions, ApikeyAuth>(ApikeyAuth.SchemeName, null);

        builder.Services.SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = false;
            o.DocumentSettings = s =>
            {
                s.AddAuth(ApikeyAuth.SchemeName, new()
                {
                    Name = ApikeyAuth.HeaderName,
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.ApiKey,
                });
            };
        });

        var connectionString = builder.Configuration.GetConnectionString("FoodService")
            ?? throw new InvalidOperationException("Connection string 'FoodService' not found.");

        builder.Services.AddInfrastructure(connectionString);

        builder.Services.AddTransient<IRestaurantRepository, RestaurantRepository>();
        builder.Services.AddTransient<IOrderRepository, OrderRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwaggerGen();

        app.UseFastEndpoints();

        app.UseHttpsRedirection();


        app.Run();
    }
}