namespace Backend.DTOs;

public class ForgotPasswordRequestDTO
{
    public string Email { get; }
    
    public ForgotPasswordRequestDTO(string email)
    {
        Email = email;
    }
}