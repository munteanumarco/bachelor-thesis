namespace API.Configuration;

public static class ApplicationConfiguration
{
    public static void ConfigureApp(WebApplication app)
    {
        app.UseCors("AllowSpecificOrigin");
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}