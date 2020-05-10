using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories.Impl
{
    public class SalesAggregateRepository :
        AbstractEFRepository<SalesDbContext, Guid, SalesAggregateEntity>, ISalesAggregateRepository,
        IDisposable
    {
        public SalesAggregateRepository(IServiceScopeFactory scopeFactory, WeekCounter counter, IMapper mapper) : base(scopeFactory)
        {
            _counter = counter;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SalesAggregate>> FetchFromLastWeekAsync(
            CancellationToken token = default)
        {
            var weekNumber = _counter.Current();
            var entities = await ListAsync(s => s.WeekNumber == weekNumber, null, true, token)
                .ConfigureAwait(false);
            return entities.Select(s => _mapper.Map<SalesAggregate>(s));
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

        protected override Task<IEnumerable<SalesAggregateEntity>> ListAsync(
            Expression<Func<SalesAggregateEntity, bool>> predicate = null,
            string sortField = null,
            bool isAscending = true,
            Func<IQueryable<SalesAggregateEntity>, IQueryable<SalesAggregateEntity>>
                queryableUpdateFunc = null,
            CancellationToken token = default)
        {
            return base.ListAsync(predicate, sortField, isAscending,
                q =>
                {
                    queryableUpdateFunc ??= same => same;
                    return queryableUpdateFunc(q).Include(p => p.Product);
                }, token);
        }

        public override string ToString()
        {
            return $"In memory {nameof(SalesAggregateRepository)}";
        }

        private readonly WeekCounter _counter;

        private readonly IMapper _mapper;
        protected override DbSet<SalesAggregateEntity> Entities => DbContext.SaleAggregates;

        
    }
}