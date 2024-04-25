using API.Middleware;

namespace API.Configuration;

public static class ApplicationConfiguration
{
    public static async void ConfigureApp(WebApplication app)
    {
        app.UseCors("AllowSpecificOrigin");
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<RequestContextMiddleware>();
        app.MapControllers();
    }
}