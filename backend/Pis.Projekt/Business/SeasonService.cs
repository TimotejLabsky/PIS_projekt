using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Calendar;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.Domain.Repositories.Impl;

// ReSharper disable All

namespace Pis.Projekt.Business
{
    public class SeasonService
    {
        public SeasonService(WsdlCalendarService calendar,
            SalesEvaluatorService evaluator,
            AggregateFetcher fetcher,
            IOptimizationNotificationService notificationService,
            IServiceScopeFactory scopeFactory,
            PriceCalculatorService calculatorService,
            IMapper mapper,
            UserTaskManager taskManager,
            WaiterService waiter,
            IOptions<StoreConfiguration> storeConfiguration,
            ILogger<SeasonService> logger,
            StoreService store)
        {
            _calendar = calendar;
            _evaluator = evaluator;
            _fetcher = fetcher;
            _notificationService = notificationService;
            _scopeFactory = scopeFactory;
            _calculatorService = calculatorService;
            _mapper = mapper;
            _taskManager = taskManager;
            _waiter = waiter;
            _storeConfiguration = storeConfiguration.Value;
            _logger = logger;
            _store = store;
        }

        public async Task Handle()
        {
            using var scope = _scopeFactory.CreateScope();
            var seasonRepository =
                scope.ServiceProvider.GetRequiredService<ISeasonRepository>();
            var productRepository =
                scope.ServiceProvider.GetRequiredService<ISalesAggregateRepository>();

            var currentDate = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);

            var season = await _fetcher.FetchSeasonAsync(currentDate, seasonRepository);

            _logger.LogBusinessCase(BusinessTasks.FetchAllProducts);
            _logger.LogInput(BusinessTasks.FetchAllProducts, "Aktualna sezona", season);
            var aggregates = await productRepository.FetchFromLastWeekAsync(0)
                .ConfigureAwait(false);
            //TODO look at this
            var allProducts = aggregates.Select(a => new TaskProduct
            {
                Id = a.Id,
                Price = a.Price,
                SaleCoefficient = a.SaleCoefficient,
                SoldAmount = a.SoldAmount,
                IsSeasonal = false,
                Product = a.Product
            });
            _logger.LogOutput(BusinessTasks.FetchAllProducts, "Vsetky produkty", allProducts);

            // human task for picking new products for this season
            _logger.LogBusinessCase(BusinessTasks.PickSeasonProducts);
            _logger.LogInput(BusinessTasks.PickSeasonProducts, "Aktualna sezona", season);
            _logger.LogInput(BusinessTasks.PickSeasonProducts, "Vsetky produkty", allProducts);

            var pickedProducts =
                await pickSeasonProducts(season, allProducts).ConfigureAwait(false);

            _logger.LogOutput(BusinessTasks.PickSeasonProducts, "Zoznam sezonnych produktov",
                pickedProducts);
            //lower the prices by 10%

            _logger.LogBusinessCase(BusinessTasks.AdjustPrices);
            _logger.LogInput(BusinessTasks.AdjustPrices, "Zoznam sezonnych produktov",
                pickedProducts);
            pickedProducts = AdjustPrices(pickedProducts);
            _logger.LogOutput(BusinessTasks.AdjustPrices, "Upraveny zoznam sezonnych produktov",
                pickedProducts);

            //nofify advertisment 
            _logger.LogBusinessCase(BusinessTasks.NotifyUpdatedSeasonPrices);
            _logger.LogInput(BusinessTasks.NotifyUpdatedSeasonPrices, "Zoznam sezonnych produktov",
                pickedProducts);
            await _notificationService.NotifyUpdatedSeasonPrices(pickedProducts);

            _logger.LogBusinessCase(BusinessTasks.EvaluateSeasonSales);
            _logger.LogInput(BusinessTasks.EvaluateSeasonSales, "Zoznam sezonnych produktov",
                pickedProducts);
            var result = _evaluator.EvaluateSeasonSales(pickedProducts);
            _logger.LogOutput(BusinessTasks.EvaluateSeasonSales,
                "Zoznam produktov so zníženou predajnosťou", result.DecreasedSales);
            _logger.LogOutput(BusinessTasks.EvaluateSeasonSales, "Ostatne produkty",
                result.OtherSales);

            // Exclusion gate
            var gateProducts = result.OtherSales;
            if (result.DecreasedSales.Any())
            {
                _logger.LogDecisionBlock("Znizila sa predajnost produktu o viac ako 20%", "Ano");

                // branch Ano
                _logger.LogBusinessCase(BusinessTasks.DecreaseSeasonalProductsPrices);
                _logger.LogInput(BusinessTasks.DecreaseSeasonalProductsPrices,
                    "Zoznam sezonnych produktov s predajnostou znizenou o 20%", pickedProducts);
                var modifiedProducts = DecreaseProductsPrices(pickedProducts);
                _logger.LogOutput(BusinessTasks.DecreaseSeasonalProductsPrices,
                    "Upraveny zoznam sezonnych produktov s predajnostou znizenou o 20% so znizenou cenou o 50%",
                    pickedProducts);

                _logger.LogBusinessCase(BusinessTasks.NotifyUpdatedSeasonPrices);
                _logger.LogInput(BusinessTasks.NotifyUpdatedSeasonPrices,
                    "Upraveny zoznam sezonnych produktov s predajnostou znizenou o 20% so znizenou cenou o 50%",
                    pickedProducts);
                await _notificationService.NotifyUpdatedSeasonPrices(pickedProducts);
                gateProducts = modifiedProducts;
            }
            else
            {
                // branch Nie
            }

            await _waiter.WaitSeasonStartAsync().ConfigureAwait(false);

            await ApplyChanges(gateProducts).ConfigureAwait(false);

            foreach (var store in _storeConfiguration.StoreEmails)
            {
                // notify applied changes... each store
                await _notificationService.NotifyStoreChangedPrices(store).ConfigureAwait(false);
                // external call store to check supplies in storage
                await _store.CheckStockings(store).ConfigureAwait(false);
            }
        }

        private IEnumerable<TaskProduct> DecreaseProductsPrices(
            IEnumerable<TaskProduct> seasonalProducts)
        {
            foreach (var product in seasonalProducts)
            {
                product.Price = product.Price.Half();
            }

            return seasonalProducts;
        }

        private async Task<IEnumerable<TaskProduct>> pickSeasonProducts(Season season,
            IEnumerable<TaskProduct> allProducts)
        {
            var result = await _taskManager
                .ExecuteUserTask(UserTaskType.IncludeToSeason, allProducts)
                .ConfigureAwait(false);
            return result.Where(s => s.IsSeasonal == true);
        }

        private IEnumerable<TaskProduct> AdjustPrices(IEnumerable<TaskProduct> products)
        {
            foreach (var product in products)
            {
                product.Price = _calculatorService.CalculatePrice(product);
            }

            return products;
        }

        private async Task ApplyChanges(IEnumerable<TaskProduct> seasonalProducts)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IPricedProductRepository>();
            var repoSeasonal =
                scope.ServiceProvider.GetRequiredService<SeasonalProductRepository>();

            foreach (var product in seasonalProducts)
            {
                var guid = Guid.NewGuid();
                await repo.CreateAsync(new PricedProductEntity
                {
                    Id = guid,
                    Price = product.Price,
                    Product = _mapper.Map<ProductEntity>(product.Product),
                    ProductGuid = product.Product.Id,
                    SalesWeek = ++product.SalesWeek,
                }).ConfigureAwait(false);

                await repoSeasonal.CreateAsync(new SeasonalProductEntity
                {
                    PricedProductGuid = guid
                }).ConfigureAwait(false);
            }
        }

        private readonly IOptimizationNotificationService _notificationService;
        private readonly PriceCalculatorService _calculatorService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SalesEvaluatorService _evaluator;
        private readonly WsdlCalendarService _calendar;
        private readonly UserTaskManager _taskManager;
        private readonly AggregateFetcher _fetcher;
        private readonly WaiterService _waiter;
        private readonly IMapper _mapper;
        private readonly StoreService _store;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly ILogger<SeasonService> _logger;
    }

    public static class DecimalExtensions
    {
        public static decimal Half(this decimal @this)
        {
            return @this * new decimal(0.5);
        }
    }

    public class StoreConfiguration
    {
        public IEnumerable<string> StoreEmails { get; set; }
    }
}