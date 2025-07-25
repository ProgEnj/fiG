namespace Backend.ErrorHandling;

public static class AuthenticationErrors
{
    public static readonly Error UserAlreadyExist = new("User already exist");
    public static readonly Error Identity = new("Identity error");
    public static readonly Error UserNotFound = new("User not found");
    public static readonly Error InvalidLogin = new("Incorrect email or password");
    public static readonly Error WrongToken = new("Tokens do not match");
    public static readonly Error ConfirmationEmail = new("Failed to confirm email");
    public static readonly Error ConfirmedEmail = new("Email is not confirmed");
    public static readonly Error PasswordChange = new("Failed to change password");
}