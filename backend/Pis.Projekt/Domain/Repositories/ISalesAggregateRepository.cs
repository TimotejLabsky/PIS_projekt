using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories
{
    public interface ISalesAggregateRepository : IRepository<Guid, SalesAggregateEntity>, ITransactionalRepository
    {
        Task<IEnumerable<SalesAggregateEntity>> FetchFromLastWeekAsync(CancellationToken token =
            default);

        Task<SalesAggregateEntity> RequireAsync(
            Expression<Func<SalesAggregateEntity, bool>> predicate,
            CancellationToken token);
    }
}