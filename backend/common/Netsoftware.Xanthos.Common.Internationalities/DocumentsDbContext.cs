using Microsoft.EntityFrameworkCore;
using Netsoftware.Xanthos.Common.Internationalities.Models;

namespace Netsoftware.Xanthos.Common.Internationalities;

public class DocumentsDbContext : DbContext
{
    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
    {
    }

    public DbSet<CultureInfo> CultureInfos { get; set; }
    public DbSet<CurrencyInfo> CurrencyInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // entityType.Relational().TableName = "Documents_" + entityType.Relational().TableName;
        }
    }
}