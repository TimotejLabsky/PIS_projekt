using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Calendar;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.System;

namespace Pis.Projekt.Business
{
    // Done
    public class SalesOptimalizationService
    {
        public SalesOptimalizationService(WaiterService waiter,
            TaskHandlerService taskScheduler,
            ProductPersistenceService productPersistence,
            ISalesAggregateRepository aggregateRepository,
            SalesEvaluatorService evaluator,
            IOptimizationNotificationService notificationService,
            ILogger<SalesOptimalizationService> logger,
            WsdlCalendarService calendar,
            AggregateFetcher fetcher,
            ParallelTaskService parallelTaskService)
        {
            _waiter = waiter;
            _taskScheduler = taskScheduler;
            _productPersistence = productPersistence;
            _aggregateRepository = aggregateRepository;
            _evaluator = evaluator;
            _notificationService = notificationService;
            _logger = logger;
            _calendar = calendar;
            _fetcher = fetcher;
            _parallelTaskService = parallelTaskService;
        }

        public async Task OptimizeSalesAsync(CancellationToken token = default)
        {
            var currentDate = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);
            var products = await _fetcher
                .FetchSalesAggregatesAsync(currentDate, _aggregateRepository, token)
                .ConfigureAwait(false);
            var evaluationResult = _evaluator.EvaluateSales(products);
            // Split -> send to hosted service
            // Branch increased
            var increasedSalesTask =
                _taskScheduler.StartIncreasedSalesTask(evaluationResult.IncreasedSales);
            // Branch B save to Db
            _logger.LogBusinessCase("Getting products with decreased sales");
            var decreasedSalesTask =
                _taskScheduler.StartDecreasedSalesTask(evaluationResult.DecreasedSales);
            await ParallelTaskService.ExecuteAsync(increasedSalesTask, decreasedSalesTask)
                .ConfigureAwait(false);
            await _waiter.WaitAsync();

            _logger.LogDevelopment("Code section");

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
        private readonly ILogger<SalesOptimalizationService> _logger;
        private readonly ParallelTaskService _parallelTaskService;
        private readonly TaskHandlerService _taskScheduler;
        private readonly SalesEvaluatorService _evaluator;
        private readonly WsdlCalendarService _calendar;
        private readonly AggregateFetcher _fetcher;
        private readonly WaiterService _waiter;
    }
}