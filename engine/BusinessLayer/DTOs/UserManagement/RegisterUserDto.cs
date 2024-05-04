using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.UserManagement;

public class RegisterUserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}