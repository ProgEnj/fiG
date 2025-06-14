using Microsoft.AspNetCore.Identity;

namespace Backend.Identity;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}