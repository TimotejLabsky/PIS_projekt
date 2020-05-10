using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories
{
    public interface ISeasonPricedProductRepository : IRepository<Guid, SeasonPricedProductEntity>, ITransactionalRepository
    {
        Task<IEnumerable<SeasonPricedProduct>> FetchFromLastSeasonAsync(Season season, CancellationToken token =
            default);
    }
}