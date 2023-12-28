using System.Reflection;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

namespace API.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
    }
}