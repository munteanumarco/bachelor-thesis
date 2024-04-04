using System.Text.Json.Serialization;

namespace DataAccessLayer.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    [JsonIgnore]
    public string Email { get; set; } = null!;  
    public string Password { get; set; } = null!;
}