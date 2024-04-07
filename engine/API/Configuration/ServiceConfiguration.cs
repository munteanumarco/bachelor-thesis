using System.Text;
using BusinessLayer.Settings;
using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.EventContracts;
using BusinessLayer.Services;
using DataAccessLayer.DbContexts;
using DataAccessLayer.Entities;
using dotenv.net;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace API.Configuration;

//TO-DO: refactor this (e.g. structure the setup in a more readable way, extract the setup of services to separate methods, etc.)
public static class ServiceConfiguration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {"Configuration/.env"}));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input bellow.\r\n\r\nExample: \"Bearer 1safsadasf\"",
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        var baseAppSettings = new BaseAppSettings();
        baseAppSettings.FrontendBaseUrl = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? baseAppSettings.FrontendBaseUrl;
        
        builder.Services.AddSingleton(baseAppSettings);
        
        builder.Services.AddIdentity<EmergencyAppUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<EngineServiceContext>()
        .AddDefaultTokenProviders();
        
        var jwtSettings = new JwtSettings();
        jwtSettings.JwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? jwtSettings.JwtIssuer;
        jwtSettings.JwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? jwtSettings.JwtAudience;
        jwtSettings.JwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? jwtSettings.JwtKey;
        jwtSettings.JwtAlgorithm = Environment.GetEnvironmentVariable("JWT_ALGORITHM") ?? jwtSettings.JwtAlgorithm;
        
        builder.Services.AddSingleton(jwtSettings);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options=>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.JwtIssuer,
                ValidAudience = jwtSettings.JwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtKey))
            };
        });
        
        var rabbitMqSettings = new RabbitMQSettings();
        rabbitMqSettings.HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? rabbitMqSettings.HostName;
        rabbitMqSettings.Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? rabbitMqSettings.Port.ToString());
        rabbitMqSettings.UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? rabbitMqSettings.UserName;
        rabbitMqSettings.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? rabbitMqSettings.Password;
    
        builder.Services.AddSingleton(rabbitMqSettings);

        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.HostName, h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });
                cfg.Message<UserCreatedEvent>(m => m.SetEntityName("UserCreatedEvent"));
                cfg.Message<ResetPasswordEvent>(m => m.SetEntityName("ResetPasswordEvent"));
            });
        });
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/engine-service.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
        builder.Services.AddSingleton<Serilog.ILogger>(provider => Log.Logger);
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
    }
}