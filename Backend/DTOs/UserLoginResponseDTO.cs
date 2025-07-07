namespace Backend.DTOs;

public class UserLoginResponseDTO
{
    public string Token { get; }

    public UserLoginResponseDTO(string token)
    {
        Token = token;
    }
}
