namespace Netsoftware.Xanthos.Common.Resources.Configurations;

public class TokenConfiguration
{
    public int ExpiryTimeInMinutes { get; set; }
    public string JwtKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }

    public TokenValidationConfiguration TokenValidationConfiguration { get; set; }
}

public class TokenValidationConfiguration
{
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
}