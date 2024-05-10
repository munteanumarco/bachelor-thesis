using API.Responses;
using BusinessLayer.DTOs.UserManagement;
using BusinessLayer.Interfaces;
using BusinessLayer.Settings;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly GoogleSettings _googleSettings;

    public UserController(IUserService userService, IAuthService authService, GoogleSettings googleSettings)
    {
        _userService = userService;
        _authService = authService;
        _googleSettings = googleSettings;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginUserDto userDto)
    {
        try
        {
            var result = await _userService.LoginAsync(userDto);
            if (!result.IsSuccess) return BadRequest(LoginResponse.Failure(result.Errors));

            var user = result.Data;
            var roles = await _userService.GetRolesAsync(user);
            var tokenString = await _authService.GenerateJwtToken(user, roles);

            return Ok(LoginResponse.Success(tokenString));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> RegisterUser([FromBody] RegisterUserDto newUser)
    {
        var result = await _userService.CreateUserAsync(newUser);
        if (!result.IsSuccess) return BadRequest(RegisterResponse.Failure(result.Errors));

        var createdUser = result.Data;

        await _userService.PublishUserCreatedEventAsync(createdUser);

        return Created("api/users", RegisterResponse.Success(createdUser));
    }

    [HttpPost("confirm-email")]
    public async Task<ActionResult<BaseResponse>> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
    {
        var result = await _userService.ConfirmEmailAsync(confirmEmailDto);
        if (!result.IsSuccess) return BadRequest(BaseResponse.Failure(result.Errors));
        return Ok(BaseResponse.Success());
    }

    [HttpPost("send-reset-password-email")]
    public async Task<ActionResult<BaseResponse>> ResetPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        await _userService.PublishResetPasswordEventAsync(forgotPasswordDto.Email);
        return Ok(BaseResponse.Success());
    }

    [HttpPost("set-new-password")]
    public async Task<ActionResult<BaseResponse>> SetNewPassword([FromBody] SetNewPasswordDto setNewPasswordDto)
    {
        var result = await _userService.SetNewPassword(setNewPasswordDto);
        if (!result.IsSuccess) return BadRequest(BaseResponse.Failure(result.Errors));
        return Ok(BaseResponse.Success());
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] LoginGoogleDto dto)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { this._googleSettings.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.Credentials, settings);

            var result = await _userService.GetUserByEmailAsync(payload.Email);
            
            if (!result.IsSuccess)
            {
                var newUser = new RegisterUserDto()
                {
                    Email = payload.Email,
                    Username = await _userService.GenerateUsername(payload.Email),
                    Password = "A" + Guid.NewGuid().ToString()
                };
                
                result = await _userService.CreateUserAsync(newUser);
                
                if (!result.IsSuccess) return BadRequest(result.Errors);
            }
            
            var user = result.Data;
            var roles = await _userService.GetRolesAsync(user);
            var tokenString = await _authService.GenerateJwtToken(user, roles);

            return Ok(LoginResponse.Success(tokenString));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}