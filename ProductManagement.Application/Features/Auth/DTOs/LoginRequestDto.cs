using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.Features.Auth.DTOs;

public class LoginRequestDto
{

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = default!;
}
