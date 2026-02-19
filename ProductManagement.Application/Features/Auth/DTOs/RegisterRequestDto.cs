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

    public bool ReceiveNewsletter { get; set; }

    public string Role { get; set; } = "User";

    //public string FirstName { get; set; } = default!;
    //public string LastName { get; set; } = default!;
    //public string UserName { get; set; } = default!;
    //public string Email { get; set; } = default!;
    //public string PhoneNumber { get; set; } = default!;

    //public string Password { get; set; } = default!;
    //public string ConfirmPassword { get; set; } = default!;

    //public DateTime DateOfBirth { get; set; }
    //public string Gender { get; set; } = default!;
    //public string Country { get; set; } = default!;
    //public string Address { get; set; } = default!;
    //public string AboutMe { get; set; } = default!;
    //public bool ReceiveNewsletter { get; set; }

    //public string Role { get; set; } = "User";
}
