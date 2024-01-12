using Netsoftware.Xanthos.Common.Resources.Utils;

namespace Netsoftware.Xanthos.Common.Resources.Login;

public class LoginResource
{
    public string Login { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string AppId { get; set; }
    public int CompanyId { get; set; }
#nullable enable
    public ApplicationBaseRoles? Role { get; set; }
#nullable disable
}