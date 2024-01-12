namespace Netsoftware.Xanthos.Common.Resources.Login;

public class ResetPasswordResource
{
    public ResetPasswordResource(string Email, string Token, string Password)
    {
        this.Email = Email;
        this.Token = Token;
        NewPassword = Password;
    }

    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}