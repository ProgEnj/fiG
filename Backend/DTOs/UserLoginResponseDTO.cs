namespace Backend.DTOs;

public class UserLoginResponseDTO
{
    public string Token { get; }
    public string Username { get; }

    public UserLoginResponseDTO(string token, string username)
    {
        Token = token;
        Username = username;
    }
}
