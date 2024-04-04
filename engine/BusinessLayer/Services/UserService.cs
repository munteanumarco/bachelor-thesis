using System.Web;
using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;
    private readonly IConfiguration _configuration;
    
    public UserService(IUserRepository userRepository, IMapper mapper,  Serilog.ILogger logger, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task<LoginResponse> LoginAsync(LoginUserDto userDto)
    {
        try
        {
            var user = await _userRepository.GetUserByIdentifier(userDto.UserIdentifier);
            
            if (user == null)
            {
                _logger.Error("Invalid credentials.");
                return LoginResponse.Failure("Invalid credentials.");
            }
           
            if (user.IsBlocked)
            {
                _logger.Error("User is blocked.");
                return LoginResponse.Failure("User is blocked.");
            }

            if (!user.EmailConfirmed) 
            {
                _logger.Error("Email not confirmed.");
                return LoginResponse.Failure("Email not confirmed.");
            }
            
            var isPasswordCorrect = await _userRepository.LoginAsync(user, userDto.Password);
            if (isPasswordCorrect)  return LoginResponse.Success("");
            
            _logger.Error("Invalid credentials.");
            return LoginResponse.Failure("Invalid credentials.");
        }
        catch (BaseException ex)
        {
            _logger.Error(ex, "An error occurred while logging in");
            throw;
        }
    }
    
    public async Task<EmergencyAppUser> GetUserByIdentifier(string userIdentifier)
    {
        try
        {
            return await _userRepository.GetUserByIdentifier(userIdentifier);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting the user");
            throw;
        }
    }
    
    public async Task<IList<string>> GetRolesAsync(EmergencyAppUser user)
    {
        return await _userRepository.GetRolesAsync(user);
    }
    
    public async Task<IdentityResult> CreateUserAsync(RegisterUserDto newUser)
    {
        try
        {
            var user = _mapper.Map<EmergencyAppUser>(newUser);
            var userCreated = await _userRepository.CreateUserAsync(user, newUser.Password);
            
            // Send email confirmation

            return userCreated;
                
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    
}