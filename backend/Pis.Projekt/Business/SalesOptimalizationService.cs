using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            ISalesAggregateRepository aggregateRepository,
            ProductPersistenceService productPersistence,
            DecreasedSalesHandler decreasedSalesHandler,
            IncreasedSalesHandler increasedSalesHandler,
            ILogger<SalesOptimalizationService> logger,
            SalesEvaluatorService evaluator,
            WsdlCalendarService calendar,
            AggregateFetcher fetcher)
        {
            _waiter = waiter;
            _productPersistence = productPersistence;
            _aggregateRepository = aggregateRepository;
            _evaluator = evaluator;
            _notificationService = notificationService;
            _logger = logger;
            _calendar = calendar;
            _fetcher = fetcher;
            _decreasedSalesHandler = decreasedSalesHandler;
            _increasedSalesHandler = increasedSalesHandler;
        }

        public async Task OptimizeSalesAsync(CancellationToken token = default)
        {
            var currentDate = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);
            var products = await _fetcher
                .FetchSalesAggregatesAsync(currentDate, _aggregateRepository, token)
                .ConfigureAwait(false);
            var evaluationResult = _evaluator.EvaluateSales(products);
            var increasedSalesTask = _increasedSalesHandler.Handle(evaluationResult.IncreasedSales);
            var decreasedSalesTask = _decreasedSalesHandler.Handle(evaluationResult.DecreasedSales);
            await Task.WhenAll(increasedSalesTask, decreasedSalesTask).ConfigureAwait(false);
            await _waiter.WaitAsync();
            // capture results from parallel tasks
            var increasedList = increasedSalesTask.Result;
            var decreasedList = decreasedSalesTask.Result;
            _logger.LogDebug($"Products with increased sales: {increasedList}");
            _logger.LogDebug($"Products with decreased sales: {decreasedList}");
            var newPriceList = increasedList.Concat(decreasedList).ToList();

            _logger.LogBusinessCase("Persisting new data to storage");
            await _productPersistence.PersistProductsAsync(newPriceList, token)
                .ConfigureAwait(false);
            //
            // _logger.LogBusinessCase("Scheduling next optimization task");
            _logger.LogBusinessCase("Sending notification about finished optimization");
            await _notificationService.NotifyOptimizationFinishedAsync(DateTime.Now)
                .ConfigureAwait(false);
        }

        private readonly IOptimizationNotificationService _notificationService;
        private readonly ISalesAggregateRepository _aggregateRepository;
        private readonly ProductPersistenceService _productPersistence;
        private readonly DecreasedSalesHandler _decreasedSalesHandler;
        private readonly IncreasedSalesHandler _increasedSalesHandler;
        private readonly ILogger<SalesOptimalizationService> _logger;
        private readonly SalesEvaluatorService _evaluator;
        private readonly WsdlCalendarService _calendar;
        private readonly AggregateFetcher _fetcher;
        private readonly WaiterService _waiter;
    }
}