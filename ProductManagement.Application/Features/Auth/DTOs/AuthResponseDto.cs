namespace ProductManagement.Application.Features.Auth.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime Expiry { get; set; }
}
