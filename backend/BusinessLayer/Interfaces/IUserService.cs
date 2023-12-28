using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IUserService
{
    Task<IEnumerable<GetUserDto>> GetUsersAsync();
}