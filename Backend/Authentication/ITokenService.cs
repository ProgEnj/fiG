using Backend.Identity;

namespace Backend.Authentication;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);

    string GenerateRefreshToken();
}