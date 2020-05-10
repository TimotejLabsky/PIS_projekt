using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories
{
    public interface ISalesAggregateRepository : IRepository<Guid, SalesAggregateEntity>, ITransactionalRepository
    {
        Task<IEnumerable<SalesAggregate>> FetchFromLastWeekAsync(int week,
            CancellationToken token = default);

        Task<SalesAggregateEntity> RequireAsync(
            Expression<Func<SalesAggregateEntity, bool>> predicate,
            CancellationToken token);
    }
}