using API.Configuration;

var builder = WebApplication.CreateBuilder(args);

ServiceConfiguration.ConfugreServices(builder);

var app = builder.Build();

ApplicationConfiguration.ConfigureApp(app);

app.Run();