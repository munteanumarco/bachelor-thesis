using API.Responses;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    
    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody]LoginUserDto userDto)
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
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> RegisterUser([FromBody] RegisterUserDto newUser)
    {
        var result = await _userService.CreateUserAsync(newUser);
        if (!result.IsSuccess) return BadRequest(RegisterResponse.Failure(result.Errors));
        
        var createdUser = result.Data;
        
        await _userService.PublishUserCreatedEventAsync(createdUser);
        
        return Created("api/users", RegisterResponse.Success(createdUser));
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("confirm-email")]
    public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail([FromBody]ConfirmEmailDto confirmEmailDto)
    {
        var result = await _userService.ConfirmEmailAsync(confirmEmailDto);
        if (!result.IsSuccess) return BadRequest(ConfirmEmailResponse.Failure(result.Errors));
        return Ok(ConfirmEmailResponse.Success());
    }
    
}