using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Netsoftware.Xanthos.Common.Resources.Interfaces;

namespace Netsoftware.Xanthos.Database.Repositories;

public interface IGenericRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "",
        int? limit = null);

    Task<TEntity> GetAsync(object id);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemovePermamentlyAsync(object entity);
    Task RemovePermamentlyAsync(TEntity entity);
    Task RemoveAsync(object entity);
    Task RemoveAsync(IDeletable entity);
}