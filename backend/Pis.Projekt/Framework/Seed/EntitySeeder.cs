using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Framework.Seed
{
    public class EntitySeeder
    {
        public EntitySeeder(IOptions<EntitySeederConfiguration> configuration,
            ILogger<EntitySeeder> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _configuration = configuration.Value;
        }
        
        public async Task Seed()
        {
            using var scope = _scopeFactory.CreateScope();
            var salesAggregateRepository = scope.ServiceProvider.GetRequiredService<ISalesAggregateRepository>();
            if (_configuration.SeedAtStart)
            {
                _logger.LogDebug($"Seeding of entities by {nameof(EntitySeeder)} has started");
                var weekAmount = _configuration.WeekAmount;
                var products = _configuration.ProductNames;
                var minValue = _configuration.PriceMin;
                var maxValue = _configuration.PriceMax;

                var salesCoefMin = _configuration.SalesCoefMin;
                var salesCoefMax = _configuration.SalesCoefMax;

                using var saleAggregateEnumerator =
                    CreateSaleAggregates(weekAmount, products, minValue, maxValue,salesCoefMin, salesCoefMax).GetEnumerator();
                while (saleAggregateEnumerator.MoveNext())
                {
                    var salesAggregate = saleAggregateEnumerator.Current;
                    if (salesAggregate != null)
                    {
                        await salesAggregateRepository.CreateAsync(salesAggregate)
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
            double minValue, double maxValue, double salesCoefMin, double salesCoefMax)
        {
            using var productEnumerator = CreateProducts(products).GetEnumerator();
            while (productEnumerator.MoveNext())
            {
                for (uint i = 0; i < weekAmount; i++)
                {
                    if (productEnumerator.Current != null)
                    {
                        var product = productEnumerator.Current;
                        var aggregate = new SalesAggregateEntity
                        {
                            Id = Guid.NewGuid(),
                            Price = new decimal(new Random().NextDouble() * (maxValue - minValue) +
                                                minValue),
                            Product = product,
                            ProductGuid = product.Id,
                            SaleCoefficient = new decimal(new Random().NextDouble() * (salesCoefMax - salesCoefMin) +
                                                          salesCoefMin),
                            WeekNumber = i,
                            //TODO change it if you have time and mood
                            SoldAmount = new Random().Next(50,500),

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
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EntitySeederConfiguration _configuration;
    }
}