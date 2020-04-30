using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Pis.Projekt.Framework.Repositories
{
    public abstract class AbstractEFRepository<TDbContext, TID, TEntityModel>
        : IRepository<TID, TEntityModel>, ITransactionalRepository, IDisposable
        where TDbContext : DbContext
        where TEntityModel : class, IEntity<TID>
    {
        protected TDbContext DbContext { get; }
        protected abstract DbSet<TEntityModel> Entities { get; }

        protected AbstractEFRepository(IServiceScopeFactory scopeFactory)
        {
            _isDisposed = false;
            Scope = scopeFactory.CreateScope();
            DbContext = Scope.ServiceProvider.GetRequiredService<TDbContext>();
        }

        public virtual async Task<IEnumerable<TEntityModel>> ListAsync(
            Expression<Func<TEntityModel, bool>> predicate = null,
            string sortField = null, bool isAscending = true,
            CancellationToken token = default)
        {
            return await ListAsync(predicate, sortField, isAscending, null, token)
                .ConfigureAwait(false);
        }

        public virtual async Task<TEntityModel> FindAsync(TID id, CancellationToken token = default)
        {
            return await FindAsync(id, null, token)
                .ConfigureAwait(false);
        }

        public virtual async Task<TEntityModel> RequireAsync(TID id, CancellationToken token = default)
        {
            return await RequireAsync(id, null, token)
                .ConfigureAwait(false);
        }

        public virtual async Task<TEntityModel> CreateAsync(TEntityModel entity, CancellationToken token = default)
        {
            if (DbContext.Exists(entity))
            {
                // when entity exist in database context, there is nothing to create...
                throw new InvalidOperationException("Entity already exists in database context.");
            }

            await Entities.AddAsync(entity, token)
                .ConfigureAwait(false);
            await SaveChangesAsync(token)
                .ConfigureAwait(false);

            return entity;
        }

        public virtual async Task<TEntityModel> UpdateAsync(TEntityModel entity, CancellationToken token = default)
        {
            if (!DbContext.Exists(entity))
            {
                // when entity does not exist in database context, there is nothing to update...
                throw new InvalidOperationException("Entity does not exist in database context.");
            }

            await SaveChangesAsync(token)
                .ConfigureAwait(false);

            return entity;
        }

        public virtual async Task<bool> RemoveAsync(TID id, CancellationToken token = default)
        {
            var deletedEntity = await Entities
                .FirstOrDefaultAsync(m => m.Id.Equals(id), token)
                .ConfigureAwait(false);

            if (deletedEntity == null)
            {
                // when deleted entity is not found,
                // signalize failure via return value
                return false;
            }

            // delete and save changes (handle throwing NotFoundException when requested)
            Entities.Remove(deletedEntity);
            await DbContext.SaveChangesAsync(token)
                .ConfigureAwait(false);

            return true;
        }

        #region --- ITransactionalRepository implementation ---

        public async Task<object> BeginTransactionAsync(CancellationToken token = default)
        {
            return await DbContext.Database.BeginTransactionAsync(token)
                .ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync(object transaction, CancellationToken token = default)
        {
            if (!(transaction is IDbContextTransaction dbContextTransaction))
            {
                throw new ArgumentException("Expected IDbContextTransaction instance.", nameof(transaction));
            }
            
            await dbContextTransaction.CommitAsync(token)
                .ConfigureAwait(false);
        }

        public async Task RollbackTransactionAsync(object transaction, CancellationToken token = default)
        {
            if (!(transaction is IDbContextTransaction dbContextTransaction))
            {
                throw new ArgumentException("Expected IDbContextTransaction instance.", nameof(transaction));
            }
            
            await dbContextTransaction.RollbackAsync(token)
                .ConfigureAwait(false);
        }

        #endregion
        
        #region --- Protected methods ---

        protected virtual async Task<IEnumerable<TEntityModel>> ListAsync(
            Expression<Func<TEntityModel, bool>> predicate = null,
            string sortField = null, bool isAscending = true,
            Func<IQueryable<TEntityModel>, IQueryable<TEntityModel>> queryableUpdateFunc = null,
            CancellationToken token = default)
        {
            // when update func is not provided, use identity
            queryableUpdateFunc ??= q => q;

            // create queryable and perform listing
            IQueryable<TEntityModel> queryable = queryableUpdateFunc(Entities);
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            if (sortField != null)
            {
                queryable = queryable.OrderByDynamic(sortField, isAscending);
            }

            // perform query and store result entities
            return await queryable
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        protected virtual async Task<TEntityModel> FindAsync(TID id,
            Func<IQueryable<TEntityModel>, IQueryable<TEntityModel>> queryableUpdateFunc = null,
            CancellationToken token = default)
        {
            // when update func is not provided, use identity
            queryableUpdateFunc ??= q => q;
            // find entity model based on id
            return await queryableUpdateFunc(Entities)
                .FirstOrDefaultAsync(m => m.Id.Equals(id), token)
                .ConfigureAwait(false);
        }

        protected async Task<TEntityModel> RequireAsync(TID id,
            Func<IQueryable<TEntityModel>, IQueryable<TEntityModel>> queryableUpdateFunc = null,
            CancellationToken token = default)
        {
            var result = await this.FindAsync(id, queryableUpdateFunc, token)
                .ConfigureAwait(false);
            if (result == null)
            {
                throw new NotFoundException(
                    $"{typeof(TEntityModel).Name} with identifier {id.ToString()} was not found.");
            }

            return result;
        }

        protected async Task SaveChangesAsync(CancellationToken token = default)
        {
            await DbContext.SaveChangesAsync(token)
                .ConfigureAwait(false);
        }

        protected async Task TransactionAsync(Func<CancellationToken, Task> asyncTask,
            CancellationToken token = default)
        {
            await using var tx = await DbContext.Database.BeginTransactionAsync(token)
                .ConfigureAwait(false);

            try
            {
                await asyncTask(token)
                    .ConfigureAwait(false);

                await tx.CommitAsync(token)
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                await tx.RollbackAsync(token)
                    .ConfigureAwait(false);

                throw;
            }
        }

        protected async Task<TResult> TransactionAsync<TResult>(Func<CancellationToken, Task<TResult>> asyncTask,
            CancellationToken token = default)
        {
            await using var tx = await DbContext.Database.BeginTransactionAsync(token)
                .ConfigureAwait(false);

            try
            {
                var result = await asyncTask(token)
                    .ConfigureAwait(false);

                await tx.CommitAsync(token)
                    .ConfigureAwait(false);

                return result;
            }
            catch (Exception)
            {
                await tx.RollbackAsync(token)
                    .ConfigureAwait(false);

                throw;
            }
        }

        #endregion

        #region --- IDispose implementation ---

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                Scope.Dispose();
            }

            _isDisposed = true;
        }

        private bool _isDisposed;

        #endregion

        protected IServiceScope Scope;
    }

    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> q, string sortField, bool isAscending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            var method = isAscending ? "OrderBy" : "OrderByDescending";
            var types = new[] {q.ElementType, exp.Body.Type};
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}