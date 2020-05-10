using System;
using System.Threading;
using System.Threading.Tasks;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories
{
    public interface ISeasonRepository: IRepository<Guid, SeasonEntity>, ITransactionalRepository
    {
        Task<Season> FetchSeason(DateTime currentDate, CancellationToken token = default);

    }
}