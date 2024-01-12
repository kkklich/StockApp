using System;
using Netsoftware.Xanthos.Common.Resources.Utils;

namespace Netsoftware.Xanthos.Common.Resources;

public class UserResource
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Company { get; set; }
    public bool External { get; set; } = false;
    public int CompanyId { get; set; }
    public string Status { get; set; }
    public bool HasAccess { get; set; }
    public bool HasConfirmedEmail { get; set; }
    public ApplicationBaseRoles Role { get; set; }
#nullable enable
    public int? ExternalId { get; set; }
#nullable disable
    public bool IsActive { get; set; }
    public string Password { get; set; }
    public string Language { get; set; }
}