using System.Text;
using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.Consumers;
using BusinessLayer.Services;
using BusinessLayer.Settings;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using dotenv.net;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace API.Configuration;

public class ServiceConfiguration
{
    public static void ConfugreServices(WebApplicationBuilder builder)
    {
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {"Configuration/.env"}));
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("x-signalr-user-agent");
                });
        });
        
        var mailSettings = new MailSettings();
        mailSettings.Mail = Environment.GetEnvironmentVariable("MAIL") ?? mailSettings.Mail;
        mailSettings.DisplayName = Environment.GetEnvironmentVariable("MAIL_DISPLAY_NAME") ?? mailSettings.DisplayName;
        mailSettings.Password = Environment.GetEnvironmentVariable("MAIL_PASSWORD") ?? mailSettings.Password;
        mailSettings.Host = Environment.GetEnvironmentVariable("MAIL_HOST") ?? mailSettings.Host;
        mailSettings.Port = int.Parse(Environment.GetEnvironmentVariable("MAIL_PORT") ?? mailSettings.Port.ToString());

        builder.Services.AddSingleton(mailSettings);
        
        var rabbitMqSettings = new RabbitMqSettings();
        rabbitMqSettings.HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? rabbitMqSettings.HostName;
        rabbitMqSettings.Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? rabbitMqSettings.Port.ToString());
        rabbitMqSettings.UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? rabbitMqSettings.UserName;
        rabbitMqSettings.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? rabbitMqSettings.Password;
        
        builder.Services.AddSingleton(rabbitMqSettings);
        
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<UserCreatedEventConsumer>();
            x.AddConsumer<ResetPasswordEventConsumer>();
            x.AddConsumer<EmergencyReportedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.HostName, h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });
                
                cfg.ReceiveEndpoint("user-created-queue", e =>
                {
                    e.Consumer<UserCreatedEventConsumer>(context);
                    e.Bind("UserCreatedEvent");
                });
                
                cfg.ReceiveEndpoint("reset-password-queue", e =>
                {
                    e.Consumer<ResetPasswordEventConsumer>(context);
                    e.Bind("ResetPasswordEvent");
                });
                
                cfg.ReceiveEndpoint("emergency-reported-queue", e =>
                {
                    e.Consumer<EmergencyReportedEventConsumer>(context);
                    e.Bind("EmergencyReportedEvent");
                });
            });
        });
        
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
            
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/chat-hub")) // Adjust the path as per your application's URL
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/communication-service.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
        builder.Services.AddSignalR();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddSingleton<Serilog.ILogger>(provider => Log.Logger);
        builder.Services.AddSingleton<IMailSendingService, MailSendingService>();
        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddScoped<IChatRepository, ChatRepository>();
        
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
        
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
    }
}