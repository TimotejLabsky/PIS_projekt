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
    public interface IPricedProductRepository : IRepository<Guid, PricedProductEntity>,
        ITransactionalRepository
    {
        Task<PricedProductEntity> RequireAsync(
            Expression<Func<PricedProductEntity, bool>> predicate,
            CancellationToken token);

        Task<IEnumerable<PricedProductEntity>> FetchFromWeekAsync(uint weekNumber,
            CancellationToken token);

        Task<IEnumerable<PricedProductEntity>> FetchFromLastWeekAsync(CancellationToken token =
            default);
    }
}