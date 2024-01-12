using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Netsoftware.Xanthos.Common.Resources.GridResources;
using Netsoftware.Xanthos.Common.Resources.Interfaces;

namespace Netsoftware.Xanthos.ApplicationInstance.Database.Repositories;

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

    IQueryable<TEntity> GetGridTableRows(GridParamsResource gridParams,
        Expression<Func<TEntity, bool>> additionalFilters = null);

    Task<int> GetGridTableRowsCount(GridParamsResource gridParams,
        Expression<Func<TEntity, bool>> additionalFilters = null);
}