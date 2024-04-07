using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs;

public class SetNewPasswordDto
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
        
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
        
    [EmailAddress]
    public string Email { get; set; }
    public string Token { get; set; }
}