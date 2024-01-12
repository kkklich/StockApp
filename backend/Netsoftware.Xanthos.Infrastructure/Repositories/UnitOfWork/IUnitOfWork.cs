using Netsoftware.Xanthos.Database;

namespace Netsoftware.Xanthos.Infrastructure.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    ApplicationDbContext Context { get; }
    void BeginTransaction();
    void Commit();
    void Rollback();
    void Save();
}