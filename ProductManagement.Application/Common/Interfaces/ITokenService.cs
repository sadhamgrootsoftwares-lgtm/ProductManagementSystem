using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(AppUser user);
    RefreshToken GenerateRefreshToken();
}
