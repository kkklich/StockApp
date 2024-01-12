using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Netsoftware.Xanthos.Common.Resources.Interfaces;
using Netsoftware.Xanthos.Database;
using Netsoftware.Xanthos.Database.Repositories;

namespace Netsoftware.Xanthos.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "",
        int? limit = null)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (filter != null) query = query.Where(filter);

        if (limit.HasValue) query = query.Take(limit.Value);

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null)
            return await Task.FromResult(orderBy(query).ToList());
        return await Task.FromResult(query.ToList());
    }

    public async Task<TEntity> GetAsync(object id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task RemovePermamentlyAsync(object entity)
    {
        var toRemove = await _context.Set<TEntity>().FindAsync(entity);
        await RemovePermamentlyAsync(toRemove);
    }

    public async Task RemovePermamentlyAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(object entity)
    {
        var toRemove = await _context.Set<TEntity>().FindAsync(entity);
        if (toRemove == null) throw new ArgumentNullException("entity", "Entity does not exists!");
        await RemoveAsync(toRemove);
    }

    public async Task RemoveAsync(IDeletable entity)
    {
        entity.IsDelete = true;
        await UpdateAsync(entity as TEntity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}