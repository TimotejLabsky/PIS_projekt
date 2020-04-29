using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Framework
{
    public abstract class EFRepository<TDBContext, TId, TEntity>
        where TEntity : class, IEntity<TId>
        where TDBContext : DbContext
    {
        public EFRepository(TDBContext context)
        {
            DbContext = context;
        }

        public async Task<TEntity> FindAsync(TId id, CancellationToken token = default)
        {
            return await FindAsync(id, null, token).ConfigureAwait(false);
        }

        public Task<TEntity> FindAsync(TId id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> q,
            CancellationToken token = default)
        {
            q ??= q;

            return q(Entities).First(e => e.Id == id);
        }

        protected TDBContext DbContext { get; set; }
        protected DbSet<TEntity> Entities => DbContext.Set<TEntity>();
    }
}