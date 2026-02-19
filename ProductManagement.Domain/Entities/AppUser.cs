using Microsoft.AspNetCore.Identity;

namespace ProductManagement.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; } = default!;
    public string? Country { get; set; } = default!;
    public string? Address { get; set; } = default!;
    public string? AboutMe { get; set; } = default!;
    public bool ReceiveNewsletter { get; set; }
    public string? ProfilePicturePath { get; set; }
}
