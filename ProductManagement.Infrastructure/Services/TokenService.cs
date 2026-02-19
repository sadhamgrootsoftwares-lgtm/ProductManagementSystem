using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Application.Common.Interfaces;
using ProductManagement.Application.Common.Models;
using ProductManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductManagement.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<AppUser> userManager)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
    }

    public async Task<string> GenerateAccessTokenAsync(AppUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(role =>
            new Claim(ClaimTypes.Role, role)).ToList();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }
        .Union(userClaims)
        .Union(roleClaims);

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var creds = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow
                .AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiryDate = DateTime.UtcNow
                .AddDays(_jwtSettings.RefreshTokenDurationInDays),
            IsRevoked = false
        };
    }
}
