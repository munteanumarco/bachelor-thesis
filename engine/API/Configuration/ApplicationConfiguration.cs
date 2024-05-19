using API.Middleware;

namespace API.Configuration;

public static class ApplicationConfiguration
{
    public static async void ConfigureApp(WebApplication app)
    {
        app.UseCors("CorsPolicy");
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<RequestContextMiddleware>();
        app.MapControllers();
    }
}