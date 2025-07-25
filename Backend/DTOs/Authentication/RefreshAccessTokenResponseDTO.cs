namespace Backend.DTOs;

public class RefreshAccessTokenResponseDTO
{
    public string Token { get; }
    
    public RefreshAccessTokenResponseDTO(string token)
    {
        this.Token = token;
    }
}