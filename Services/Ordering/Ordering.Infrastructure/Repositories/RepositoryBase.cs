using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ordering.Domain.Common;

namespace Ordering.Infrastructure.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly OrderContext _db;
    internal DbSet<T> _dbSet;
    public RepositoryBase(OrderContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null) return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includes = null,
                                        bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;
        if (disableTracking) query = query.AsNoTracking();

        //  Aggregate : Tổng hợp
        if (includes != null) query = includes.Aggregate(query,
                            (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null) return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        _dbSet.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        // _dbSet.Update(entity);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
