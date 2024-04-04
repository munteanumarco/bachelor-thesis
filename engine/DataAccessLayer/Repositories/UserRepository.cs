using DataAccessLayer.DbContexts;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EngineServiceContext _context;
    private readonly UserManager<EmergencyAppUser> _userManager;
    private readonly Serilog.ILogger _logger;
    private readonly SignInManager<EmergencyAppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public UserRepository(EngineServiceContext context, UserManager<EmergencyAppUser> userManager, SignInManager<EmergencyAppUser> signInManager, RoleManager<IdentityRole> roleManager, Serilog.ILogger logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    
    public async Task<bool> LoginAsync(EmergencyAppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
    
    public async Task<IdentityResult> CreateUserAsync(EmergencyAppUser user, string password)
    {
        try
        {
            var userRole = RoleConstants.USER_ROLE;
            bool userRoleExists = await _roleManager.RoleExistsAsync(userRole);

            if (!userRoleExists)
            {
                _logger.Error("Error in user role management!");
                var error = new IdentityError() { Description = "Error while creating user!" };
                return IdentityResult.Failed(error);
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userRole);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error while creating user!");
            var error = new IdentityError() { Description = "Error while creating user!" };
            return IdentityResult.Failed(error);
        }
    }
    
    public async Task<EmergencyAppUser?> GetUserByIdentifier(string userIdentifier)
    {
        return await _userManager.FindByEmailAsync(userIdentifier)
               ?? await _userManager.FindByNameAsync(userIdentifier);
    }

    public async Task<IList<string>> GetRolesAsync(EmergencyAppUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}