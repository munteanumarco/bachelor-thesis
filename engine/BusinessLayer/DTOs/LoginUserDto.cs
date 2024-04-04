using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models;

public class LoginUserDto
{
    [Required]
    public string UserIdentifier { get; set; }
    [Required]
    public string Password { get; set; }
}