using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtToken(UserDto user, IList<string> roles);
}