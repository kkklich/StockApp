namespace Netsoftware.Xanthos.Common.AuthenticationMiddleware;

public class TokenResource
{
    public TokenResource(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}