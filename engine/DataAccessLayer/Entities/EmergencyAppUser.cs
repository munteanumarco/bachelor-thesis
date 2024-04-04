using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities;

public class EmergencyAppUser : IdentityUser
{
    public bool IsBlocked { get; set; }
}