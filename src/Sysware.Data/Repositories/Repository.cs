using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlSugar;

namespace Sysware.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    private readonly ISqlSugarClient _db;

    public Repository(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<TEntity?> GetByIdAsync(object id)
    {
        return await _db.Queryable<TEntity>().InSingleAsync(id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _db.Queryable<TEntity>().ToListAsync();
    }

    public async Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _db.Queryable<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await _db.Queryable<TEntity>().CountAsync();
        }
        return await _db.Queryable<TEntity>().Where(predicate).CountAsync();
    }

    public async Task<bool> InsertAsync(TEntity entity)
    {
        return await _db.Insertable(entity).ExecuteCommandAsync() > 0;
    }

    public async Task<bool> InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        return await _db.Insertable(entities.ToArray()).ExecuteCommandAsync() > 0;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        return await _db.Deleteable(entity).ExecuteCommandAsync() > 0;
    }

    public async Task<bool> DeleteByIdAsync(object id)
    {
        return await _db.Deleteable<TEntity>().In(id).ExecuteCommandAsync() > 0;
    }
}


