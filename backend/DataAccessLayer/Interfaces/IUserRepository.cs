using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
}