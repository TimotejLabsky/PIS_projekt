using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Business.Calendar;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

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
            WaiterService waiter)
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
            // human task for picking new products for this season
            var pickedProducts =
                await pickSeasonProducts(season, allProducts).ConfigureAwait(false);

            //lower the prices by 10%
            pickedProducts = AdjustPrices(pickedProducts);

            //nofify advertisment 
            await _notificationService.NotifyUpdatedSeasonPrices(pickedProducts);

            var result = _evaluator.EvaluateSeasonSales(pickedProducts);

            // Exclusion gate
            var gateProducts = result.OtherSales;
            if (result.DecreasedSales.Any())
            {
                // branch Ano
                var modifiedProducts = DecreaseProductsPrices(pickedProducts);
                await _notificationService.NotifyUpdatedSeasonPrices(pickedProducts);
                gateProducts = modifiedProducts;
            }
            else
            {
                // branch Nie
            }

            await _waiter.WaitSeasonStartAsync().ConfigureAwait(false);

            await ApplyChanges(gateProducts).ConfigureAwait(false);
            //TODO 
            //TODO 
            //TODO 
            //TODO 
            //TODO 
            //TODO 
            //TODO 
            //TODO 
            //TODO 
            // notify applied changes... each store
            
            // external call store to check supplies in storage
        }

        private IEnumerable<TaskProduct> DecreaseProductsPrices(IEnumerable<TaskProduct> seasonalProducts)
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
            var result =  await _taskManager.ExecuteUserTask(UserTaskType.IncludeToSeason, allProducts)
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

            foreach (var product in seasonalProducts)
            {
                await repo.CreateAsync(new PricedProductEntity
                {
                Id = Guid.NewGuid(),
                Price = product.Price,
                Product = _mapper.Map<ProductEntity>(product.Product),
                ProductGuid = product.Product.Id,
                SalesWeek = ++product.SalesWeek,
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
    }

    public static class DecimalExtensions
    {
        public static decimal Half(this decimal @this)
        {
            return @this * new decimal(0.5);
        }
    }
}