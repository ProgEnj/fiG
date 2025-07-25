namespace Backend.DTOs;

public class UserRegisterRequestDTO
{
    public string Email { get; }
    public string UserName { get; }
    public string Password { get; }

    public UserRegisterRequestDTO(string email, string userName, string password)
    {
        Email = email;
        UserName = userName;
        Password = password;
    }
}