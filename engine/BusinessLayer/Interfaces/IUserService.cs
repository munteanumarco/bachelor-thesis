using BusinessLayer.DTOs.UserManagement;
using BusinessLayer.Helpers;

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