using System;
using Netsoftware.Xanthos.Common.Resources.Interfaces;

namespace Netsoftware.Xanthos.ApplicationInstance.Database.Models;

public class ApplicationInstance : IDeletable
{
    public ApplicationInstance()
    {
        CreationDate = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string CompanyName { get; set; }
    public string Name { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid ModificatedBy { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
    public bool IsDelete { get; set; }
}