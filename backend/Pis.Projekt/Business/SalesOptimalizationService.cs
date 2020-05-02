using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Business
{
    // Done
    public class SalesOptimalizationService
    {
        public SalesOptimalizationService(WaiterService waiter,
            // CronSchedulerService cronScheduler,
            // TaskSchedulerService taskScheduler,
            ProductPersistenceService productPersistence,
            ISalesAggregateRepository aggregateRepository,
            IMapper mapper,
            SalesEvaluatorService evaluator,
            IOptimizationNotificationService notificationService, ILogger<SalesOptimalizationService> logger)
        {
            _waiter = waiter;
            // _cronScheduler = cronScheduler;
            // _taskScheduler = taskScheduler;
            _productPersistence = productPersistence;
            _aggregateRepository = aggregateRepository;
            _mapper = mapper;
            _evaluator = evaluator;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task OptimizeSalesAsync(CancellationToken token = default)
        {
            _logger.LogInformation("Sending notification about beginning of optimization");
            await _notificationService.NotifyOptimizationBegunAsync().ConfigureAwait(false);
            _logger.LogInformation("Fetching sales from last week");
            var products = await FetchSalesAggregatesAsync(token)
                .ConfigureAwait(false);
            
            _logger.LogInformation("Evaluating sales of products");
            var evaluationResult = _evaluator.EvaluateSales(products);
            //
            //
            // var tasks = new List<Task>();
            // // Split -> send to hosted service
            // // Branch increased
            // _logger.LogInformation("Getting products with increased sales");
            // var increasedSalesTask =
            //     _taskScheduler.RegisterIncreasedSalesTask(evaluationResult.IncreasedSales);
            // tasks.Add(increasedSalesTask);
            // // Branch B save to Db
            // _logger.LogInformation("Getting products with decreased sales");
            // var decreasedSalesTask =
            //     _taskScheduler.RegisterDecreasedSalesTask(evaluationResult.DecreasedSales);
            // tasks.Add(decreasedSalesTask);
            //
            // await Task.WhenAll(tasks);
            // await _waiter.WaitAsync();
            // var increasedList = increasedSalesTask.Result;
            // var decreasedList = decreasedSalesTask.Result;
            // _logger.LogDebug($"Products with increased sales: {increasedList}");
            // _logger.LogDebug($"Products with decreased sales: {decreasedList}");
            // var newPriceList = increasedList.Concat(decreasedList).ToList();
            //
            // _logger.LogInformation("Persisting new data to storage");
            // await _productPersistence.PersistProductsAsync(newPriceList, token).ConfigureAwait(false);
            //
            // _logger.LogInformation("Scheduling next optimization task");
            // var nextOptimalizationOn = await _cronScheduler.ScheduleNextOptimalizationTask(token)
            //     .ConfigureAwait(false);
            //
            // _logger.LogInformation("Sending notification about finished optimization");
            // await _notificationService.NotifyOptimizationFinishedAsync(nextOptimalizationOn)
            //     .ConfigureAwait(false);
        }

        /// <summary>
        /// System nacita vsetky produkty z uloziska za minuly tyzden aj zo vsetkymi aggregovanymi udajmi
        /// </summary>
        /// <remarks>
        /// Aggregovany udaj je cena, predajnost etc...
        /// </remarks>
        /// <returns></returns>
        private async Task<IEnumerable<SalesAggregate>> FetchSalesAggregatesAsync(
            CancellationToken token = default)
        {
            var sales = await _aggregateRepository
                .FetchFromLastWeekAsync(token)
                .ConfigureAwait(false);
            _logger.LogTrace($"Fetched sales: {sales}");
            return _mapper.Map<IEnumerable<SalesAggregate>>(sales);
        }

        private readonly ISalesAggregateRepository _aggregateRepository;
        private readonly ProductPersistenceService _productPersistence;
        private readonly IOptimizationNotificationService _notificationService;
        // private readonly CronSchedulerService _cronScheduler;
        // private readonly TaskSchedulerService _taskScheduler;
        private readonly SalesEvaluatorService _evaluator;
        private readonly WaiterService _waiter;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesOptimalizationService> _logger;
    }
}