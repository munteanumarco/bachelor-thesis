using System.Web;
using AutoMapper;
using BusinessLayer.DTOs.UserManagement;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using BusinessLayer.RabbitMQ.EventContracts;
using BusinessLayer.Settings;
using DataAccessLayer.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly UserManager<EmergencyAppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly BaseAppSettings _appSettings;
    public UserService(IMapper mapper,  Serilog.ILogger logger, IPublishEndpoint publishEndpoint, UserManager<EmergencyAppUser> userManager, RoleManager<IdentityRole> roleManager, BaseAppSettings appSettings)
    {
        _mapper = mapper;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _userManager = userManager;
        _roleManager = roleManager;
        _appSettings = appSettings;
    }

    public async Task<string> GenerateUsername(string email)
    {
        List<string> allUsernames = await _userManager.Users.Select(u => u.UserName).ToListAsync();

        string[] parts = email.Split('@');
        string baseUsername = parts[0];
        string username = baseUsername;

        int suffix = 1;
        
        while (allUsernames.Contains(username))
        {
            username = baseUsername + suffix;
            suffix++;
        }

        return username;
    }

    public async Task<OperationResult<UserDto>> GetUserByEmailAsync(string email)
    {
        var user = await  GetEntityUserAsync(email);
        return user == null ? OperationResult<UserDto>.Failure(new List<string>(){"User not found"}) : OperationResult<UserDto>.Success(_mapper.Map<UserDto>(user));
    }

    public async Task<OperationResult<UserDto>> LoginAsync(LoginUserDto userDto)
    {
        try
        {
            var user =  await GetEntityUserAsync(userDto.UserIdentifier);
            
            if (user == null) return OperationResult<UserDto>.Failure(new List<string>(){"Invalid credentials."});
           
            if (user.IsBlocked) return OperationResult<UserDto>.Failure(new List<string>(){"User is blocked."});

            if (!user.EmailConfirmed) return OperationResult<UserDto>.Failure(new List<string>(){"Email not confirmed."});
            
            if (await _userManager.CheckPasswordAsync(user, userDto.Password)) return OperationResult<UserDto>.Success(_mapper.Map<UserDto>(user));
            
            return OperationResult<UserDto>.Failure(new List<string>(){"Invalid credentials."});
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while logging in");
            throw;
        }
    }
    
    public async Task<OperationResult<UserDto>> CreateUserAsync(RegisterUserDto userDto)
    {
        try
        {
            var userEntity = _mapper.Map<EmergencyAppUser>(userDto);
            const string userRole = RoleConstants.USER_ROLE;
            
            var createResult = await _userManager.CreateAsync(userEntity, userDto.Password);
            if (!createResult.Succeeded)
            {
                return OperationResult<UserDto>.Failure(createResult.Errors.Select(e => e.Description));
            }
            
            var addToRoleResult = await _userManager.AddToRoleAsync(userEntity, userRole);
            if (!addToRoleResult.Succeeded)
            {
                return OperationResult<UserDto>.Failure(addToRoleResult.Errors.Select(e => e.Description));
            }
            
            return OperationResult<UserDto>.Success(_mapper.Map<UserDto>(userEntity));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating a user");
            throw;
        }
    }
    
    public async Task PublishUserCreatedEventAsync(UserDto userDto)
    {
        try
        {
            var userEntity = await GetEntityUserAsync(userDto.Email);
            if (userEntity == null) throw new Exception("User not found");    
            var emailConfirmationLink = await GetEmailConfirmationLinkAsync(userEntity);

            await _publishEndpoint.Publish(new UserCreatedEvent()
            {
                Email = userEntity.Email,
                Username = userEntity.UserName,
                ConfirmationLink = emailConfirmationLink
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while publishing user created event");
            throw;
        }
    }
    
    public async Task PublishResetPasswordEventAsync(string email)
    {
        try
        {
            var userEntity = await GetEntityUserAsync(email);
            if (userEntity == null) return;
            var token = await _userManager.GeneratePasswordResetTokenAsync(userEntity);
            var resetLink = _appSettings.FrontendBaseUrl + "/reset-password?token=" + HttpUtility.UrlEncode(token) + "&email=" + HttpUtility.UrlEncode(userEntity.Email);
            await _publishEndpoint.Publish(new ResetPasswordEvent()
            {
                Username = userEntity.UserName,
                Email = userEntity.Email,
                ResetLink = resetLink
            });
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while publishing reset password event");
            throw;
        }
    }
    
    public async Task<IList<string>> GetRolesAsync(UserDto userDto)
    {
        try
        {
            var userEntity = await GetEntityUserAsync(userDto.Email);
            if (userEntity == null) throw new Exception("User not found");
            return await _userManager.GetRolesAsync(userEntity);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting user roles");
            throw;
        }
    }
    
    public async Task<OperationResult<UserDto>> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
    {
        try
        {
            var user = await GetEntityUserAsync(HttpUtility.UrlDecode(confirmEmailDto.Email));
            
            if (user == null) return OperationResult<UserDto>.Failure(new List<string>(){"User not found"});
            
            var result = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(confirmEmailDto.Token));
            
            if (!result.Succeeded) return OperationResult<UserDto>.Failure(new List<string>(){"Invalid credentials."});
            
            return OperationResult<UserDto>.Success(_mapper.Map<UserDto>(user));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while confirming email");
            throw;
        }
    }

    public async Task<OperationResult<string>> SetNewPassword(SetNewPasswordDto setNewPasswordDto)
    {
        try
        {
            var userEntity = await GetEntityUserAsync(HttpUtility.UrlDecode(setNewPasswordDto.Email));
            if (userEntity == null) return OperationResult<string>.Failure(new List<string>(){"User not found"});
            var result = await _userManager.ResetPasswordAsync(userEntity, HttpUtility.UrlDecode(setNewPasswordDto.Token), setNewPasswordDto.Password);
            if (!result.Succeeded) return OperationResult<string>.Failure(result.Errors.Select(e => e.Description));
            return OperationResult<string>.Success("Password reset successfully.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while setting new password");
            throw;
        }
    }

    private async Task<string> GetEmailConfirmationLinkAsync(EmergencyAppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
        return _appSettings.FrontendBaseUrl + "/confirm-email?token=" + HttpUtility.UrlEncode(token) + "&email=" + HttpUtility.UrlEncode(user.Email);
    }
    
    private async Task<EmergencyAppUser?> GetEntityUserAsync(string userIdentifier)
    {
        return await _userManager.FindByEmailAsync(userIdentifier)
                    ?? await _userManager.FindByNameAsync(userIdentifier);
    }
    
}