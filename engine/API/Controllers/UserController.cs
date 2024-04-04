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
            var response = await _userService.LoginAsync(userDto);
            if (!response.IsSuccess) return BadRequest(response);
            
            var user = await _userService.GetUserByIdentifier(userDto.UserIdentifier);
            var roles = await _userService.GetRolesAsync(user);
            var tokenString = await _authService.GenerateTokenString(user, roles);
            response.Token = tokenString;
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto newUser)
    {
        var result = await _userService.CreateUserAsync(newUser);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok("User created!");
    }
}