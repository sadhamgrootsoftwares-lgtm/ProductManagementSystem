using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Application.Common.Interfaces;
using ProductManagement.Application.Features.Auth.DTOs;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _context = context;
    }

    // ---------------- REGISTER ----------------
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto model)
    {
        if (model.Password != model.ConfirmPassword)
            return BadRequest("Passwords do not match.");

        var user = new AppUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            Country = model.Country,
            Address = model.Address,
            AboutMe = model.AboutMe,
            ReceiveNewsletter = model.ReceiveNewsletter
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            await _roleManager.CreateAsync(
                new IdentityRole(model.Role));
        }

        await _userManager.AddToRoleAsync(user, model.Role);

        return Ok("User registered successfully.");
    }

    // ---------------- LOGIN ----------------
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto model)
    {
        //var user = await _userManager.FindByEmailAsync(model.Email);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            user = await _userManager.FindByNameAsync(model.Email);
        }

        if (user == null)
            return Unauthorized("Invalid credentials.");

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
            return Unauthorized("Invalid credentials.");

        var accessToken = await _tokenService
            .GenerateAccessTokenAsync(user);

        var refreshToken = _tokenService.GenerateRefreshToken();
        refreshToken.UserId = user.Id;

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            Expiry = refreshToken.ExpiryDate
        });
    }

    // ---------------- REFRESH TOKEN ----------------
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        RefreshTokenRequestDto model)
    {
        var principal = GetPrincipalFromExpiredToken(model.AccessToken);

        if (principal == null)
            return BadRequest("Invalid access token.");

        var userId = principal.FindFirst(
            System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
            return Unauthorized();

        var storedRefreshToken = _context.RefreshTokens
            .FirstOrDefault(rt =>
                rt.Token == model.RefreshToken &&
                rt.UserId == user.Id);

        if (storedRefreshToken == null)
            return Unauthorized("Invalid refresh token.");

        if (storedRefreshToken.IsRevoked)
            return Unauthorized("Token already revoked.");

        if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
            return Unauthorized("Refresh token expired.");

        //  Rotate Token
        storedRefreshToken.IsRevoked = true;

        var newAccessToken =
            await _tokenService.GenerateAccessTokenAsync(user);

        var newRefreshToken =
            _tokenService.GenerateRefreshToken();

        newRefreshToken.UserId = user.Id;

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            Expiry = newRefreshToken.ExpiryDate
        });
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(
    string token)
    {
        var jwtSettings = HttpContext.RequestServices
            .GetRequiredService<
                Microsoft.Extensions.Options.IOptions<
                    ProductManagement.Application.Common.Models.JwtSettings>>()
            .Value;

        var tokenValidationParameters =
            new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, //  important

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key))
            };

        var tokenHandler =
            new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out var securityToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }

}

