namespace BusinessLayer.Settings;

public class JwtSettings
{
    public string JwtKey { get; set; } = "key";
    public string JwtIssuer { get; set; } = "http://localhost:5000";
    public string JwtAudience { get; set; } = "http://localhost:4200";
    public string JwtAlgorithm { get; set; } = "HS512";
}