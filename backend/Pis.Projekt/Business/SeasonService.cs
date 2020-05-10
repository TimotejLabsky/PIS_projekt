using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        public SeasonService(WsdlCalendarService calendar, SalesEvaluatorService evaluator, AggregateFetcher fetcher, IOptimizationNotificationService notificationService, IServiceScopeFactory scopeFactory, PriceCalculatorService calculatorService, IMapper mapper)
        {
            _calendar = calendar;
            _evaluator = evaluator;
            _fetcher = fetcher;
            _notificationService = notificationService;
            _scopeFactory = scopeFactory;
            _calculatorService = calculatorService;
            _mapper = mapper;
        }
        
        public async Task Handle()
        {
            using var scope = _scopeFactory.CreateScope();
            var seasonRepository =
                scope.ServiceProvider.GetRequiredService<ISeasonRepository>();
            var productRepository =
                scope.ServiceProvider.GetRequiredService<ISalesAggregateRepository>();
            var currentDate = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);
            
            var season = await _fetcher.FetchSeasonAsync(currentDate,seasonRepository);

            //TODO look at this
            var allProducts = await productRepository.FetchFromLastWeekAsync()
                .ConfigureAwait(false);
            // human task for picking new products for this season
            var pickedProducts = await pickSeasonProducts(season, allProducts).ConfigureAwait(false);
            //lower the prices by 10%
            pickedProducts = AdjustPrices(pickedProducts);
            
            //nofify advertisment 
            var result = _evaluator.EvaluateSeasonSales(pickedProducts);
            
            
            //handle decreased price product list
            
            



            // var products = await fetchProducts().ConfigureAwait(false);
            // var pickedProducts = await pickSeasonProducts(season, products).ConfigureAwait(false);
            // var modifiedProducts = await decreaseProductsPrices(pickedProducts).ConfigureAwait(false);
            // await sendProducts(modifiedProducts).ConfigureAwait(false);
            // var evaluateResult = await evaluateSales(modifiedProducts).ConfigureAwait(false);
            // var increaseSalesProducts = evaluateResult.IncreasedSalesProducts;
            // var decreaseSalesProducts = evaluateResult.DecreasedSalesProducts;
            // var mergedSalesProduct = await persistProducts(increaseSalesProducts, decreaseSalesProducts).ConfigureAwait(false);
            // await notifyStores(mergedSalesProduct);
        }

        private async Task<IEnumerable<SeasonPricedProduct>> pickSeasonProducts(Season season,
            IEnumerable<SalesAggregate> allProducts)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<SeasonPricedProduct> AdjustPrices(IEnumerable<SeasonPricedProduct> products)
        {
            foreach (var product in products)
            {
                var pricedProduct = _mapper.Map<PricedProduct>(product); 
                product.PricedProductEntity.Price = _calculatorService.CalculatePrice(pricedProduct);
            }
            return products;
        }
        
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SalesEvaluatorService _evaluator;
        private readonly WsdlCalendarService _calendar;
        private readonly AggregateFetcher _fetcher;
        private readonly IOptimizationNotificationService _notificationService;
        private readonly PriceCalculatorService _calculatorService;
        private readonly IMapper _mapper;


    }
}