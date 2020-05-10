using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SeasonPricedProductRepository : AbstractEFRepository<SalesDbContext, Guid, SeasonPricedProductEntity>,
        ISeasonPricedProductRepository,
        IDisposable
    {
        public SeasonPricedProductRepository(IServiceScopeFactory scopeFactory, WeekCounter counter, Mapper mapper) : base(
            scopeFactory)
        {
            _counter = counter;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeasonPricedProduct>> FetchFromLastSeasonAsync(Season season,
            CancellationToken token = default)
        {
            var entities = await ListAsync(s => s.SeasonId.Equals(season.Id)
                    , null, true, token).ConfigureAwait(false);
            return entities.Select(s => _mapper.Map<SeasonPricedProduct>(s));
        }
        
        protected override DbSet<SeasonPricedProductEntity> Entities => DbContext.SeasonPricedProducts;
        private readonly Mapper _mapper;
        private readonly WeekCounter _counter;
    }
}