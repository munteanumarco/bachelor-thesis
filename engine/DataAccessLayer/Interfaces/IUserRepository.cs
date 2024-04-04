using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task<bool> LoginAsync(EmergencyAppUser userIdentifier, string password);
    Task<EmergencyAppUser?> GetUserByIdentifier(string userIdentifier);
    Task<IList<string>> GetRolesAsync(EmergencyAppUser user);
    Task<IdentityResult> CreateUserAsync(EmergencyAppUser user, string password);
}