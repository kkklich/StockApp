namespace Netsoftware.Xanthos.Common.Resources;

public class ContactResource
{
    public ContactResource()
    {
        IsActive = true;
    }

    public int Id { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public int CompanyId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone1 { get; set; }
    public string Phone2 { get; set; }

    public string Email { get; set; }

    public string PreviousEmail { get; set; }

    public string Role { get; set; }

    public bool HasAccess { get; set; }

#nullable enable
    public int? WebUserExternalId { get; set; }
#nullable disable
}