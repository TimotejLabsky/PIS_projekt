using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Calendar;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Business
{
    // Done
    public class SalesOptimalizationService
    {
        public SalesOptimalizationService(WaiterService waiter,
            IOptimizationNotificationService notificationService,
            DecreasedSalesHandler decreasedSalesHandler,
            IncreasedSalesHandler increasedSalesHandler,
            ILogger<SalesOptimalizationService> logger,
            SalesEvaluatorService evaluator,
            WsdlCalendarService calendar,
            AggregateFetcher fetcher,
            IServiceScopeFactory scopeFactory)
        {
            _waiter = waiter;
            _evaluator = evaluator;
            _notificationService = notificationService;
            _logger = logger;
            _calendar = calendar;
            _fetcher = fetcher;
            _scopeFactory = scopeFactory;
            _decreasedSalesHandler = decreasedSalesHandler;
            _increasedSalesHandler = increasedSalesHandler;
        }

        public async Task OptimizeSalesAsync(CancellationToken token = default)
        {
            // inject and arrange
            using var scope = _scopeFactory.CreateScope();
            var salesRepository =
                scope.ServiceProvider.GetRequiredService<ISalesAggregateRepository>();
            var productPersistence =
                scope.ServiceProvider.GetRequiredService<ProductPersistenceService>();
            var currentDate = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);

            // act
            var products = await _fetcher
                .FetchSalesAggregatesAsync(currentDate, salesRepository)
                .ConfigureAwait(false);
            var evaluationResult = _evaluator.EvaluateSales(products);
            var increasedSalesTask = _increasedSalesHandler.Handle(evaluationResult.IncreasedSales);
            var decreasedSalesTask = _decreasedSalesHandler.Handle(evaluationResult.DecreasedSales);
            await Task.WhenAll(decreasedSalesTask).ConfigureAwait(false);
            await _waiter.WaitAsync().ConfigureAwait(false);
            // capture results from parallel tasks
            var increasedList = increasedSalesTask.Result;
            var decreasedList = decreasedSalesTask.Result;
            // _logger.LogDebug($"Products with increased sales: {increasedList}");
            _logger.LogDebug($"Products with decreased sales: {decreasedList}");
            var newPriceList = increasedList.Concat(decreasedList).ToList();
            var merged = newPriceList.Concat(evaluationResult.SameSales);
            
            await productPersistence.PersistProductsAsync(merged, token)
                .ConfigureAwait(false);
            _logger.LogBusinessCase("Sending notification about finished optimization");
            await _notificationService.NotifyOptimizationFinishedAsync(merged)
                .ConfigureAwait(false);
        }

        private readonly IOptimizationNotificationService _notificationService;
        private readonly DecreasedSalesHandler _decreasedSalesHandler;
        private readonly IncreasedSalesHandler _increasedSalesHandler;
        private readonly ILogger<SalesOptimalizationService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SalesEvaluatorService _evaluator;
        private readonly WsdlCalendarService _calendar;
        private readonly AggregateFetcher _fetcher;
        private readonly WaiterService _waiter;
    }
}