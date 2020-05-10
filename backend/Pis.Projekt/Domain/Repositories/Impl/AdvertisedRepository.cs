using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories.Impl
{
    public class AdvertisedRepository : AbstractEFRepository<SalesDbContext,Guid,AdvertisedProductEntity>
    {
        public AdvertisedRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            // empty
        }

        protected override DbSet<AdvertisedProductEntity> Entities => DbContext.Advertised;
    }
}