using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.Resources;

public class UserGeneralResource
{
    public UserGeneralResource()
    {
        Permissions = new List<string>();
        UserRoles = new List<string>();
    }

    public List<string> Permissions { get; set; }
    public List<string> UserRoles { get; set; }
}