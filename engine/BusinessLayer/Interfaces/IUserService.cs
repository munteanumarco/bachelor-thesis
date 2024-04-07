using BusinessLayer.DTOs;
using BusinessLayer.Models;
using DataAccessLayer.Entities;

namespace BusinessLayer.Interfaces;

public interface IUserService
{
    Task<OperationResult<UserDto>> LoginAsync(LoginUserDto user);
    Task<OperationResult<UserDto>> CreateUserAsync(RegisterUserDto newUser);
    Task<OperationResult<UserDto>> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
    Task<OperationResult<string>> SetNewPassword(SetNewPasswordDto setNewPasswordDto);
    Task<IList<string>> GetRolesAsync(UserDto user);
    Task PublishUserCreatedEventAsync(UserDto user);
    Task PublishResetPasswordEventAsync(string email);
}