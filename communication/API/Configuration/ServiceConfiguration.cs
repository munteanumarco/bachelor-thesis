using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.Consumers;
using BusinessLayer.Services;
using BusinessLayer.Settings;
using dotenv.net;
using MassTransit;
using Serilog;

namespace API.Configuration;

public class ServiceConfiguration
{
    public static void ConfugreServices(WebApplicationBuilder builder)
    {
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {"Configuration/.env"}));
        
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
            });
        });
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/engine-service.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
        builder.Services.AddSingleton<Serilog.ILogger>(provider => Log.Logger);
        builder.Services.AddSingleton<IMailSendingService, MailSendingService>();
        
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
    }
}