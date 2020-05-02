using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Framework.Seed
{
    public class EntitySeeder
    {
        public EntitySeeder(IProductRepository productRepository,
            ISalesAggregateRepository salesAggregateRepository)
        {
            _productRepository = productRepository;
            _salesAggregateRepository = salesAggregateRepository;
        }
        
        public async Task Seed(uint weekAmount, IEnumerable<string> products, double minValue = 10.25,
            double maxValue = 100.54)
        {
            using var saleAggregateEnumerator =
                CreateSaleAggregates(weekAmount, products, minValue, maxValue).GetEnumerator();
            while (saleAggregateEnumerator.MoveNext())
            {
                var salesAggregate = saleAggregateEnumerator.Current;
                if (salesAggregate != null)
                {
                    await _productRepository.CreateAsync(salesAggregate.Product).ConfigureAwait(false);
                    await _salesAggregateRepository.CreateAsync(salesAggregate).ConfigureAwait(false);
                }
            }
        }

        private IEnumerable<ProductEntity> CreateProducts(IEnumerable<string> products)
        {
            using var productEnumerator = products.GetEnumerator();
            while (productEnumerator.MoveNext())
            {
                yield return new ProductEntity
                {
                    Id = Guid.NewGuid(),
                    Name = productEnumerator.Current
                };
            }
        }

        private IEnumerable<SalesAggregateEntity> CreateSaleAggregates(uint weekAmount, IEnumerable<string> products,
            double minValue, double maxValue)
        {
            using var productEnumerator = CreateProducts(products).GetEnumerator();
            while (productEnumerator.MoveNext())
            {
                for (uint i = 0; i < weekAmount; i++)
                {
                    if (productEnumerator.Current != null)
                        yield return new SalesAggregateEntity
                        {
                            Id = Guid.NewGuid(),
                            Price = new decimal(new Random().NextDouble() * (maxValue - minValue) + minValue),
                            Product = productEnumerator.Current,
                            ProductGuid = productEnumerator.Current.Id,
                            SaleCoefficient = new decimal(new Random().NextDouble()),
                            WeekNumber = i
                        };
                }
            }
        }

        private readonly IProductRepository _productRepository;
        private ISalesAggregateRepository _salesAggregateRepository;
    }
}