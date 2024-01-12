namespace Netsoftware.Xanthos.Common.Resources.Login;

public class ExternalLoginResource
{
    public ExternalLoginResource()
    {
        LoginResource = new LoginResource();
    }

    public LoginResource LoginResource { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Company { get; set; }
}