namespace Backend.DTOs;

public class UserLoginRequestDTO
{
    public string Email { get; }
    public string Password { get; }

    public UserLoginRequestDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }
}