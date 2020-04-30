using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Business
{
    public class ProductPersistenceService
    {
        public ProductPersistenceService(IMapper mapper,
            IPricedProductRepository pricedRepository,
            WeekCounter weekCounter,
            ILogger<ProductPersistenceService> logger)
        {
            _mapper = mapper;
            _pricedRepository = pricedRepository;
            _weekCounter = weekCounter;
            _logger = logger;
        }

        public async Task PersistProductsAsync(IEnumerable<PricedProduct> modifiedProducts,
            CancellationToken token = default)
        {
            var transaction = (IDbContextTransaction) await _pricedRepository
                .BeginTransactionAsync(token)
                .ConfigureAwait(false);

            var currentWeek = _weekCounter.Current();
            try
            {
                var existingPricedProducts = (await _pricedRepository
                    .ListAsync(s =>
                        s.SalesWeek == currentWeek, token: token)).ToList();
                if (!existingPricedProducts.Any())
                {
                    _logger.LogDebug($"Persisting process of {nameof(PricedProductEntity)} " +
                                     $"for week {currentWeek} started");
                    foreach (var modified in modifiedProducts)
                    {
                        var productEntity = _mapper.Map<ProductEntity>(modified.Product);
                        var existingPricedProduct = new PricedProductEntity
                        {
                            SalesWeek = 0,
                            Price = modified.Price,
                            Product = productEntity,
                            ProductGuid = productEntity.Guid
                        };

                        await _pricedRepository.CreateAsync(existingPricedProduct, token)
                            .ConfigureAwait(false);
                        _logger.LogDebug($"{nameof(PricedProductEntity)} " +
                                         $"with Id: {existingPricedProduct.Id} " +
                                         $"for week {currentWeek} was stored " +
                                         "in Database successfully", existingPricedProduct);
                    }
                }
                else
                {
                    foreach (var existingPricedProduct in existingPricedProducts)
                    {
                        await _pricedRepository.RemoveAsync(existingPricedProduct.Id, token)
                            .ConfigureAwait(false);
                        _logger.LogDebug($"Removing existing {nameof(PricedProductEntity)} " +
                                         $"with Id: {existingPricedProduct.Id} from week {currentWeek}",
                            existingPricedProduct);
                    }
                }

                await transaction.CommitAsync(token).ConfigureAwait(false);
                _logger.LogInformation(
                    $"{nameof(PricedProductEntity)} has been stored in database for week {currentWeek}");
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"Database Transaction failed on operation {nameof(PersistProductsAsync)}", e);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
                _logger.LogError(
                    $"Transaction rollback of {nameof(PersistProductsAsync)} was successful");
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private readonly ILogger<ProductPersistenceService> _logger;
        private readonly WeekCounter _weekCounter;
        private readonly IPricedProductRepository _pricedRepository;
        private readonly IMapper _mapper;
    }
}