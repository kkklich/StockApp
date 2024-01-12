namespace Netsoftware.Xanthos.Common.Resources.Login;

public class TokenResource
{
    public TokenResource(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}