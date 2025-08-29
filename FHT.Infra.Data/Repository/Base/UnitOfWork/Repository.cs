using FHT.Domain.Repositories.Base;
using FHT.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Infra.Data.Repository.Base.UnitOfWork
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _db;

        protected Repository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _db = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Query(bool noTracking = true)
            => noTracking ? _db.AsNoTracking() : _db.AsQueryable();

        public virtual async Task<TEntity> GetByIdAsync(long id, CancellationToken ct = default)
            => await _db.FindAsync(new object[] { id }, ct).AsTask();

        public virtual async Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default,
            bool noTracking = true,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> q = _db;
            if (includes != null)
                foreach (var inc in includes) q = q.Include(inc);
            if (noTracking) q = q.AsNoTracking();
            return await q.FirstOrDefaultAsync(predicate, ct);
        }

        public virtual async Task<List<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            CancellationToken ct = default,
            bool noTracking = true,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> q = _db;
            if (includes != null)
                foreach (var inc in includes) q = q.Include(inc);
            if (predicate != null) q = q.Where(predicate);
            if (noTracking) q = q.AsNoTracking();
            return await q.ToListAsync(ct);
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => _db.AsNoTracking().AnyAsync(predicate, ct);

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken ct = default)
            => predicate is null
                ? _db.AsNoTracking().CountAsync(ct)
                : _db.AsNoTracking().CountAsync(predicate, ct);

        public virtual Task AddAsync(TEntity entity, CancellationToken ct = default)
            => _db.AddAsync(entity, ct).AsTask();

        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
            => _db.AddRangeAsync(entities, ct);

        public virtual void Update(TEntity entity) => _db.Update(entity);
        public virtual void UpdateRange(IEnumerable<TEntity> entities) => _db.UpdateRange(entities);

        public virtual void Delete(TEntity entity) => _db.Remove(entity);
        public virtual void DeleteRange(IEnumerable<TEntity> entities) => _db.RemoveRange(entities);

        public virtual async Task DeleteByIdAsync(long id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity is not null) _db.Remove(entity);
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
