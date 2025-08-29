using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Domain.Repositories.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> Query(bool noTracking = true);

        Task<TEntity> GetByIdAsync(long id, CancellationToken ct = default);
        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default,
            bool noTracking = true,
            params Expression<Func<TEntity, object>>[] includes);

        Task<List<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            CancellationToken ct = default,
            bool noTracking = true,
            params Expression<Func<TEntity, object>>[] includes);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken ct = default);

        Task AddAsync(TEntity entity, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task DeleteByIdAsync(long id, CancellationToken ct = default);
    }
}
