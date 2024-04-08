using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.UserManagement;

public class LoginUserDto
{
    [Required]
    public string UserIdentifier { get; set; }
    [Required]
    public string Password { get; set; }
}