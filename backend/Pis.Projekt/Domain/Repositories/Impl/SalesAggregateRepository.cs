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
    public class SalesAggregateRepository :
        AbstractEFRepository<SalesDbContext, Guid, SalesAggregateEntity>, ISalesAggregateRepository,
        IDisposable
    {
        public SalesAggregateRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _scope = scopeFactory.CreateScope();
            _counter = _scope.ServiceProvider.GetRequiredService<WeekCounter>();
        }

        public async Task<IEnumerable<SalesAggregateEntity>> FetchFromLastWeekAsync(
            CancellationToken token = default)
        {
            var weekNumber = _counter.Current();
            return await ListAsync(s => s.WeekNumber == weekNumber, null, true, token)
                .ConfigureAwait(false);
        }

        public async Task<SalesAggregateEntity> RequireAsync(
            Expression<Func<SalesAggregateEntity, bool>> predicate,
            CancellationToken token)
        {
            var found = (await ListAsync(predicate, null, true, token)
                .ConfigureAwait(false)).ToList();
            if (found.Count() > 1)
            {
                throw new InvalidOperationException($"{nameof(SalesAggregateEntity)} More " +
                                                    $"then one results matching predicate found");
            }

            if (!found.Any())
            {
                throw new NotFoundException($"{nameof(SalesAggregateEntity)} Not found " +
                                            "based on predicate ");
            }

            return found.First();
        }

        public override string ToString()
        {
            return $"In memory {nameof(SalesAggregateRepository)}";
        }

        private readonly WeekCounter _counter;

        #region __ Disposable Pattern __

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _scope.Dispose();
            }

            disposed = true;
        }

        // Flag: Has Dispose already been called?
        bool disposed = false;

        #endregion

        private readonly IServiceScope _scope;
        protected override DbSet<SalesAggregateEntity> Entities => DbContext.SaleAggregates;
    }
}