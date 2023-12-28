using DataAccessLayer.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace API.Configuration;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        var host = Environment.GetEnvironmentVariable("DbHost");
        var database = Environment.GetEnvironmentVariable("DbName");
        var username = Environment.GetEnvironmentVariable("DbUser");
        var password = Environment.GetEnvironmentVariable("DbPassword");

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database) ||
            string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Database configuration is not set properly.");
        }

        var connectionString = $"Host={host};Database={database};Username={username};Password={password}";

        builder.Services.AddDbContext<EmergencyContext>(options =>
            options.UseNpgsql(connectionString));
    }
}