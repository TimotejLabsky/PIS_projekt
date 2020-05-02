using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories.Impl
{
    public class PricedProductRepository :
        AbstractEFRepository<SalesDbContext, Guid, PricedProductEntity>, IPricedProductRepository,
        IDisposable
    {
        public PricedProductRepository(IServiceScopeFactory scopeFactory, WeekCounter counter) : base(scopeFactory)
        {
            _counter = counter;
            _scope = scopeFactory.CreateScope();
        }

        public async Task<IEnumerable<PricedProductEntity>> FetchFromLastWeekAsync(
            CancellationToken token = default)
        {
            var weekNumber = _counter.Current();
            return await ListAsync(s => s.SalesWeek == weekNumber, null, true, token)
                .ConfigureAwait(false);
        }

        public async Task<PricedProductEntity> RequireAsync(
            Expression<Func<PricedProductEntity, bool>> predicate,
            CancellationToken token)
        {
            var found = (await ListAsync(predicate, null, true, token)
                .ConfigureAwait(false)).ToList();
            if (found.Count() > 1)
            {
                throw new InvalidOperationException($"{nameof(PricedProductEntity)} More " +
                                                    $"then one results matching predicate found");
            }

            if (!found.Any())
            {
                throw new NotFoundException($"{nameof(PricedProductEntity)} Not found " +
                                            "based on predicate ");
            }

            return found.First();
        }

        public async Task<IEnumerable<PricedProductEntity>> FetchFromWeekAsync(
            uint weekNumber,
            CancellationToken token)
        {
            return await ListAsync(s => s.SalesWeek == weekNumber, null, true, token)
                .ConfigureAwait(false);
        }

        private readonly WeekCounter _counter;

        public override Task<PricedProductEntity> FindAsync(Guid id, CancellationToken token = default)
        {
            return base.FindAsync(id,  token);
        }


        #region __ Disposable Pattern __

        public new void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _scope.Dispose();
            }

            _disposed = true;
        }

        // Flag: Has Dispose already been called?
        private bool _disposed;

        #endregion

        private readonly IServiceScope _scope;
        protected override DbSet<PricedProductEntity> Entities => DbContext.PricedProducts;
    }
}