using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.UserManagement;

public class LoginGoogleDto
{
    [Required]
    public string Credentials { get; set; }
}