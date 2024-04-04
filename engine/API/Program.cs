using API.Configuration;

var builder = WebApplication.CreateBuilder(args);

ServiceConfiguration.ConfigureServices(builder);
DatabaseConfiguration.ConfigureDatabase(builder);

var app = builder.Build();

ApplicationConfiguration.ConfigureApp(app);

app.Run();