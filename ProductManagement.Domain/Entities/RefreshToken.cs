namespace ProductManagement.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = default!;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }

    public string UserId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
}
