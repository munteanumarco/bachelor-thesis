using BusinessLayer.DTOs;
using BusinessLayer.Models;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Interfaces;

public interface IUserService
{
    Task<LoginResponse> LoginAsync(LoginUserDto user);
    Task<IdentityResult> CreateUserAsync(RegisterUserDto newUser);
    Task<EmergencyAppUser> GetUserByIdentifier(string userIdentifier);
    Task<IList<string>> GetRolesAsync(EmergencyAppUser user);
}