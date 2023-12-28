using DataAccessLayer.DbContexts;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EmergencyContext _context;

    public UserRepository(EmergencyContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}