using System;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Resources;

public class ApplicationInstanceResource
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string CompanyName { get; set; }
    public string Name { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid ModificatedBy { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
}