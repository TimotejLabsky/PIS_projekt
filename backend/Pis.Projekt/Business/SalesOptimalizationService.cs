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
    public class SalesOptimalizationService
    {
        public SalesOptimalizationService(NotificationService notificationService,
            WaiterService waiter,
            CronSchedulerService cronScheduler,
            TaskSchedulerService taskScheduler,
            ProductPersistenceService productPersistence,
            SalesAggregateRepository aggregateRepository,
            IMapper mapper)
        {
            _notificationService = notificationService;
            _waiter = waiter;
            _cronScheduler = cronScheduler;
            _taskScheduler = taskScheduler;
            _productPersistence = productPersistence;
            _aggregateRepository = aggregateRepository;
            _mapper = mapper;
        }

        public async Task OptimizeSalesAsync()
        {
            var products = await FetchSalesAggregatesAsync()
                .ConfigureAwait(false);
            var evaluationResult = await EvaluateSalesAsync(products)
                .ConfigureAwait(false);

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
            await _productPersistence.PersistProductsAsync(newPriceList);
            await _notificationService.NotifyAsync(
                OptimalizationFinishedStoreNotification.Create(newPriceList));
            await _cronScheduler.PlanNextOptimalization();
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

        //TODO: Business question: maybe move to class evaluator
        private Task<EvaluationResult> EvaluateSalesAsync(IEnumerable<SalesAggregate> allProducts)
        {
            return Task.Run(() => new EvaluationResult());
        }

        private readonly SalesAggregateRepository _aggregateRepository;
        private readonly ProductPersistenceService _productPersistence;
        private readonly NotificationService _notificationService;
        private readonly CronSchedulerService _cronScheduler;
        private readonly TaskSchedulerService _taskScheduler;
        private readonly WaiterService _waiter;
        private readonly IMapper _mapper;
    }
}