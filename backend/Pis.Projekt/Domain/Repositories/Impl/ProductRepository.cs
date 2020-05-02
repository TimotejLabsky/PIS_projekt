using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories.Impl
{
    public class ProductRepository :
        AbstractEFRepository<SalesDbContext, Guid, ProductEntity>, IProductRepository
    {
        public ProductRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            // empty
        }

        protected override DbSet<ProductEntity> Entities => DbContext.Products;
    }
}