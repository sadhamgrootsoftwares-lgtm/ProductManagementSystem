namespace ProductManagement.Application.Features.Auth.DTOs;

public class RefreshTokenRequestDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
