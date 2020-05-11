using System;
using System.Linq;
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
    public class SeasonRepository : AbstractEFRepository<SalesDbContext, Guid, SeasonEntity>,
        ISeasonRepository
    {
        public SeasonRepository(IServiceScopeFactory scopeFactory, IMapper mapper) : base(
            scopeFactory)
        {
            _mapper = mapper;
        }

        public async Task<Season> FetchSeason(DateTime currentDate,
            CancellationToken token = default)
        {
            var seasons = await ListAsync(p => currentDate.Date.CompareTo(p.StartAt) < 0, null,
                    true, token)
                .ConfigureAwait(false);
            return _mapper.Map<Season>(seasons.OrderBy(p => p.StartAt).First());
        }

        protected override DbSet<SeasonEntity> Entities => DbContext.Seasons;
        private readonly IMapper _mapper;
    }
}