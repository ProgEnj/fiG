using Backend.DTOs;
using Backend.ErrorHandling;
using Backend.Identity;

namespace Backend.Authentication;

public interface IAuthService
{
    Task<Result> RegisterAsync(UserRegisterRequestDTO request);
    Task<Result<UserLoginResponseDTO>> LoginAsync(UserLoginRequestDTO request);
    Task<Result> ConfirmEmailAsync(string userId, string code);
    Task<Result> SendConfirmationEmailAsync(ApplicationUser user);
    Task<Result> ForgotPasswordEmailAsync(ForgotPasswordRequestDTO request);
    Task<Result> ResetPasswordAsync(ResetPasswordRequestDTO request);
    Task<Result<UserLoginResponseDTO>> RefreshAccessTokenAsync();
    Task<Result> LogoutUserAsync();
}