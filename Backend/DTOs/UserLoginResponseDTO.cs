namespace Backend.DTOs;

public class UserLoginResponseDTO
{
    public string UserName { get; }
    public string Email { get; }
    public string Token { get; }
    public string RefreshToken { get; }

    public UserLoginResponseDTO(string userName, string email, string token, string refreshToken)
    {
        UserName = userName;
        Email = email;
        Token = token;
        RefreshToken = refreshToken;
    }
}
