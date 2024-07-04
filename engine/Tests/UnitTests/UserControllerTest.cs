using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using API.Controllers;
using API.Responses;
using BusinessLayer.DTOs.UserManagement;
using BusinessLayer.Interfaces;
using BusinessLayer.Settings;
using DataAccessLayer.Entities;
using Google.Apis.Auth;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly UserController _controller;
    private readonly GoogleSettings _googleSettings = new GoogleSettings { ClientId = "your-google-client-id" };

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockAuthService = new Mock<IAuthService>();
        _controller = new UserController(_mockUserService.Object, _mockAuthService.Object, _googleSettings);
    }

    [Fact]
    public async Task GoogleLogin_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var loginDto = new LoginGoogleDto { Credentials = "fake-google-token" };
        var googlePayload = new GoogleJsonWebSignature.Payload { Email = "test@example.com" };
        var user = new EmergencyAppUser() { Id = "123", Email = "test@example.com" };
        var roles = new[] { "User" };
        var tokenString = "fake-jwt-token";

        _mockUserService.Setup(x => x.GetUserByEmailAsync("test@example.com"))
                        .ReturnsAsync(ApiResponse<EmergencyAppUser>.Success(user));
        _mockUserService.Setup(x => x.GetRolesAsync(user))
                        .ReturnsAsync(roles);
        _mockAuthService.Setup(x => x.GenerateJwtToken(user, roles))
                        .ReturnsAsync(tokenString);

        // Act
        var result = await _controller.GoogleLogin(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(tokenString, (okResult.Value as LoginResponse));
    }

    [Fact]
    public async Task GoogleLogin_CreatesUser_WhenUserDoesNotExist()
    {
        // Arrange
        var loginDto = new LoginGoogleDto { Credentials = "fake-google-token" };
        var googlePayload = new GoogleJsonWebSignature.Payload { Email = "newuser@example.com" };
        var newUser = new EmergencyAppUser() { Id = "", Email = "newuser@example.com" };
        var roles = new[] { "User" };
        var tokenString = "fake-jwt-token";

        _mockUserService.Setup(x => x.CreateUserAsync(It.IsAny<RegisterUserDto>()))
                        .ReturnsAsync(ApiResponse<EmergencyAppUser>.Success(newUser));

        // Act
        var result = await _controller.GoogleLogin(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }
}
