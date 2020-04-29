using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business
{
    public class ProductPersistenceService
    {
        public ProductPersistenceService(IMapper mapper, SalesDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        public async Task PersistProductsAsync(IEnumerable<PricedProduct> modifiedProducts)
        {
            foreach (var modified in modifiedProducts)
            {
                var existingProduct = _dbContext.PricedProducts.Find(modified.SalesWeek);
                if (existingProduct == null)
                {
                    var productEntity = _mapper.Map<ProductEntity>(modified.Product);
                    existingProduct = new PricedProductEntity
                    {
                        SalesWeek = 0,
                        Price = modified.Price,
                        Product = productEntity,
                        ProductGuid = productEntity.Guid
                    };
                    
                    _dbContext.PricedProducts.Add(existingProduct);
                }
                else
                {
                    existingProduct.SalesWeek += 1;
                    existingProduct.Price = modified.Price;
                    
                    _dbContext.PricedProducts.Update(existingProduct);
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        private readonly SalesDbContext _dbContext;
        private readonly IMapper _mapper;
    }
}