using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using Netsoftware.Xanthos.Database;

namespace Netsoftware.Xanthos.Infrastructure.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly object _locker = new();
    private IDbContextTransaction _transaction;
    private bool _transactionInProgress;
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext context)
    {
        Context = context;       
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ApplicationDbContext Context { get; } 

    public void BeginTransaction()
    {
        _transaction = Context.Database.BeginTransaction();
        _transactionInProgress = true;
    }

    public void Commit()
    {
        lock (_locker)
        {
            if (_transactionInProgress)
            {
                _transaction.Commit();
                _transactionInProgress = false;
            }
            else
            {
                throw new TransactionException("Transaction not exist");
            }
        }
    }

    public void Rollback()
    {
        lock (_locker)
        {
            if (!_transactionInProgress) return;
            _transaction.Rollback();
            _transactionInProgress = false;
        }
    }

    public void Save()
    {
        Context.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing) Context.Dispose();
        _disposed = true;
    }
}