using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLayer.DTOs;
using BusinessLayer.Helpers;
using BusinessLayer.Settings;
using BusinessLayer.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;

    public AuthService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
        
    }

    public async Task<string> GenerateJwtToken(UserDto userDto, IList<string> roles)
    {

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, userDto.Email),
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim(CustomClaimTypes.UserId, userDto.Id.ToString())
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtKey));

        var signingCred = new SigningCredentials(securityKey, _jwtSettings.JwtAlgorithm);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            issuer: _jwtSettings.JwtIssuer,
            audience: _jwtSettings.JwtAudience,
            signingCredentials: signingCred);

        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return await Task.FromResult(tokenString);
    }
}