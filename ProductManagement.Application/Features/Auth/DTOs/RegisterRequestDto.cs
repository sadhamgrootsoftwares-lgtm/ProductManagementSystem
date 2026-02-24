using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.Features.Auth.DTOs;

public class RegisterRequestDto
{

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string AboutMe { get; set; } = string.Empty;

    [Required]
    public bool ReceiveNewsletter { get; set; }

    [Required]
    public string Role { get; set; } = "User";

}
