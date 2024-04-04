using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        this._configuration = configuration;

    }

    public async Task<string> GenerateTokenString(EmergencyAppUser user, IList<string> roles)
    {

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(SolutionConfigurationConstants.JwtIdClaim, user.Id)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "issuer";
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "audience";
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "7dc35e7e2113ea1473075565fa82986ff5f96fd0f6b5c5191b61614d2ded48707dc35e7e2113ea1473075565fa82986ff5f96fd0f6b5c5191b61614d2ded4870";
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var signingCred = new SigningCredentials(securityKey, SolutionConfigurationConstants.JwtAlgorithm);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            issuer: jwtIssuer,
            audience:jwtAudience,
            signingCredentials: signingCred);

        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return await Task.FromResult<string>(tokenString);
    }
}