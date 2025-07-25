namespace Backend.DTOs;

public class ResetPasswordRequestDTO
{
    public string Email { get; }
    public string ResetCode { get; }
    public string NewPassword { get; }

    public ResetPasswordRequestDTO(string email, string resetCode, string newPassword)
    {
        Email = email;
        this.ResetCode = resetCode;
        this.NewPassword = newPassword;
    }
}