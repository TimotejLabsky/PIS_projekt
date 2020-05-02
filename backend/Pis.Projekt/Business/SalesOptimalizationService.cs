using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories.Impl;

namespace Pis.Projekt.Business
{
    // Done
    public class SalesOptimalizationService
    {
        public SalesOptimalizationService(WaiterService waiter,
            CronSchedulerService cronScheduler,
            TaskSchedulerService taskScheduler,
            ProductPersistenceService productPersistence,
            SalesAggregateRepository aggregateRepository,
            IMapper mapper,
            SalesEvaluatorService evaluator,
            IOptimizationNotificationService notificationService)
        {
            _waiter = waiter;
            _cronScheduler = cronScheduler;
            _taskScheduler = taskScheduler;
            _productPersistence = productPersistence;
            _aggregateRepository = aggregateRepository;
            _mapper = mapper;
            _evaluator = evaluator;
            _notificationService = notificationService;
        }

        public async Task OptimizeSalesAsync(CancellationToken token = default)
        {
            await _notificationService.NotifyOptimizationBegunAsync().ConfigureAwait(false);
            var products = await FetchSalesAggregatesAsync(token)
                .ConfigureAwait(false);
            var evaluationResult = _evaluator.EvaluateSales(products);

            var tasks = new List<Task>();
            // Split -> send to hosted service
            // Branch increased
            var increasedSalesTask =
                _taskScheduler.RegisterIncreasedSalesTask(evaluationResult.IncreasedSales);
            tasks.Add(increasedSalesTask);
            // Branch B save to Db
            var decreasedSalesTask =
                _taskScheduler.RegisterDecreasedSalesTask(evaluationResult.DecreasedSales);
            tasks.Add(decreasedSalesTask);

            await Task.WhenAll(tasks);
            await _waiter.WaitAsync();
            var increasedList = increasedSalesTask.Result;
            var decreasedList = decreasedSalesTask.Result;
            var newPriceList = increasedList.Concat(decreasedList).ToList();
            await _productPersistence.PersistProductsAsync(newPriceList, token).ConfigureAwait(false);
            var nextOptimalizationOn = await _cronScheduler.ScheduleNextOptimalizationTask(token)
                .ConfigureAwait(false);
            await _notificationService.NotifyOptimizationFinishedAsync(nextOptimalizationOn)
                .ConfigureAwait(false);
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
            return _mapper.Map<IEnumerable<SalesAggregate>>(sales);
        }

        private readonly SalesAggregateRepository _aggregateRepository;
        private readonly ProductPersistenceService _productPersistence;
        private readonly IOptimizationNotificationService _notificationService;
        private readonly CronSchedulerService _cronScheduler;
        private readonly TaskSchedulerService _taskScheduler;
        private readonly SalesEvaluatorService _evaluator;
        private readonly WaiterService _waiter;
        private readonly IMapper _mapper;
    }
}