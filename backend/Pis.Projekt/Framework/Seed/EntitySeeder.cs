using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Framework.Seed
{
    public class EntitySeeder
    {
        public EntitySeeder(IOptions<EntitySeederConfiguration> configuration, IProductRepository productRepository,
            ISalesAggregateRepository salesAggregateRepository,
            ILogger<EntitySeeder> logger)
        {
            _productRepository = productRepository;
            _salesAggregateRepository = salesAggregateRepository;
            _logger = logger;
            _configuration = configuration.Value;
        }
        
        public async Task Seed()
        {
            if (_configuration.SeedAtStart)
            {
                _logger.LogDebug($"Seeding of entities by {nameof(EntitySeeder)} has started");
                var weekAmount = _configuration.WeekAmount;
                var products = _configuration.ProductNames;
                var minValue = _configuration.MinPrice;
                var maxValue = _configuration.MaxPrice;

                using var saleAggregateEnumerator =
                    CreateSaleAggregates(weekAmount, products, minValue, maxValue).GetEnumerator();
                while (saleAggregateEnumerator.MoveNext())
                {
                    var salesAggregate = saleAggregateEnumerator.Current;
                    if (salesAggregate != null)
                    {
                        await _productRepository.CreateAsync(salesAggregate.Product)
                            .ConfigureAwait(false);
                        await _salesAggregateRepository.CreateAsync(salesAggregate)
                            .ConfigureAwait(false);
                    }
                }
                _logger.LogDebug($"Seeding of entities by {nameof(EntitySeeder)} has finished");
            }
            else
            {
                _logger.LogDebug($"{nameof(EntitySeeder)} had been disabled by configuration");
            }
        }

        private IEnumerable<ProductEntity> CreateProducts(IEnumerable<string> products)
        {
            using var productEnumerator = products.GetEnumerator();
            while (productEnumerator.MoveNext())
            {
                var product = new ProductEntity
                {
                    Id = Guid.NewGuid(),
                    Name = productEnumerator.Current
                };
                _logger.LogDebug($"{nameof(EntitySeeder)} created {nameof(ProductEntity)}:" +
                                 $" {product.Id} with name: {product.Name}");
                yield return product;
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
                    {
                        var aggregate = new SalesAggregateEntity
                        {
                            Id = Guid.NewGuid(),
                            Price = new decimal(new Random().NextDouble() * (maxValue - minValue) +
                                                minValue),
                            Product = productEnumerator.Current,
                            ProductGuid = productEnumerator.Current.Id,
                            SaleCoefficient = new decimal(new Random().NextDouble()),
                            WeekNumber = i
                        };
                        _logger.LogDebug(
                            $"{nameof(EntitySeeder)} created {nameof(SalesAggregateEntity)}" +
                            $": {aggregate.Id}, Week: {aggregate.WeekNumber}, " +
                            $"Price: {aggregate.Price}, Sales Coeff: {aggregate.SaleCoefficient}, " +
                            $"ProductId: {aggregate.ProductGuid}");
                        yield return aggregate;
                    }
                }
            }
        }

        private readonly ILogger<EntitySeeder> _logger;
        private readonly IProductRepository _productRepository;
        private readonly EntitySeederConfiguration _configuration;
        private readonly ISalesAggregateRepository _salesAggregateRepository;
    }
}