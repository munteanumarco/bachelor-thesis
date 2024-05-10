using DataAccessLayer.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace API.Configuration;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("DB_NAME");
        var username = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database) ||
            string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Database configuration is not set properly.");
        }

        var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

        builder.Services.AddDbContext<CommunicationServiceContext>(options =>
            options.UseNpgsql(connectionString));
        
        ApplyMigrations(builder.Services);
    }
    
    private static void ApplyMigrations(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CommunicationServiceContext>();

        dbContext.Database.Migrate();
    }
}