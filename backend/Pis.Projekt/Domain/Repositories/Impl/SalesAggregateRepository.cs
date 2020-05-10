using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories.Impl
{
    public class SalesAggregateRepository : AbstractEFRepository<SalesDbContext, Guid, SalesAggregateEntity>, ISalesAggregateRepository,
        IDisposable
    {
        public SalesAggregateRepository(IServiceScopeFactory scopeFactory, IMapper mapper) : base(scopeFactory)
        {
            _mapper = mapper;
        }
        public async Task<IEnumerable<SalesAggregate>> FetchFromLastWeekAsync(int week,
            CancellationToken token = default)
        {
            var entities = await ListAsync(s => s.WeekNumber == week, null, true, token)
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

        private readonly IMapper _mapper;
        protected override DbSet<SalesAggregateEntity> Entities => DbContext.SaleAggregates;

        
    }
}