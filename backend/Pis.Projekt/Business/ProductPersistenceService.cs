using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Business
{
    // Done
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
            _logger.LogBusinessCase(BusinessTasks.PersistenceTask,
                "Persisting new data to storage");
            _logger.LogInput(BusinessTasks.PersistenceTask, "Zoznam produktov so zmenenou cenou",
                modifiedProducts);
            _logger.LogInput(BusinessTasks.PersistenceTask, "úložisko produktov",
                _pricedRepository);

            var currentWeek = _weekCounter.Current();

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
                        ProductGuid = productEntity.Id
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

            _logger.LogInformation(
                $"{nameof(PricedProductEntity)} has been stored in database for week {currentWeek}");
        }

        private readonly ILogger<ProductPersistenceService> _logger;
        private readonly WeekCounter _weekCounter;
        private readonly IPricedProductRepository _pricedRepository;
        private readonly IMapper _mapper;
    }
}