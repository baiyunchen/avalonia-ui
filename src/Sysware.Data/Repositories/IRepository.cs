using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sysware.Data.Repositories;

public interface IRepository<TEntity> where TEntity : class, new()
{
    Task<TEntity?> GetByIdAsync(object id);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
    Task<bool> InsertAsync(TEntity entity);
    Task<bool> InsertRangeAsync(IEnumerable<TEntity> entities);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> DeleteByIdAsync(object id);
}


