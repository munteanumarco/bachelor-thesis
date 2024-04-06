using BusinessLayer.DTOs;
using BusinessLayer.Models;

namespace BusinessLayer.Interfaces;

public interface IUserService
{
    Task<OperationResult<UserDto>> LoginAsync(LoginUserDto user);
    Task<OperationResult<UserDto>> CreateUserAsync(RegisterUserDto newUser);
    Task<OperationResult<UserDto>> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
    Task<IList<string>> GetRolesAsync(UserDto user);
    Task PublishUserCreatedEventAsync(UserDto user);
}