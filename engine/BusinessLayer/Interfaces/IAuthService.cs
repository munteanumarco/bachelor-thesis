using DataAccessLayer.Entities;

namespace BusinessLayer.Interfaces;

public interface IAuthService
{
    Task<string> GenerateTokenString(EmergencyAppUser user, IList<string> roles);
}