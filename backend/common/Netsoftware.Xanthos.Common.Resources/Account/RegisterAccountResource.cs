using System;

namespace Netsoftware.Xanthos.Common.Resources.Account;

public class RegisterAccountResource
{
    public string Id { get; set; }
    public Guid AppId { get; set; }
    public Guid? RoleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Company { get; set; }
    public int CompanyId { get; set; }
    public string Password { get; set; }
#nullable enable
    public int? ExternalId { get; set; }
#nullable disable
}