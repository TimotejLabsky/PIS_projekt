using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiitTaskList;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Calendar;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories.Impl;
using Pis.Projekt.Framework;
using Pis.Projekt.System;
using Task = System.Threading.Tasks.Task;

// ReSharper disable PossibleMultipleEnumeration

namespace Pis.Projekt.Business
{
    public class DecreasedSalesHandler
    {
        public DecreasedSalesHandler(SupplierService supplier,
            UserTaskCollectionService taskCollection,
            ITaskClient taskClient,
            CronSchedulerService cronScheduler,
            ILogger<DecreasedSalesHandler> logger,
            IOptimizationNotificationService notificationService,
            WsdlCalendarService calendar,
            IServiceScopeFactory scopeFactory)
        {
            _supplier = supplier;
            _taskCollection = taskCollection;
            _taskClient = taskClient;
            _cronScheduler = cronScheduler;
            _logger = logger;
            _notificationService = notificationService;
            _calendar = calendar;
            _scopeFactory = scopeFactory;
        }

        public async Task<IEnumerable<PricedProduct>> Handle(IEnumerable<TaskProduct> decreasedList)
        {
            _logger.LogBusinessCase(BusinessTasks.DecreasedSalesBranch);
            if (decreasedList.Any())
            {
                _logger.LogDecisionBlock(BusinessTasks.DecreasedSaleOfAtLeastOneProduct, "Ano");
                await NotifyMarketing().ConfigureAwait(false);
                var newPriceList = (await EvaluatePriceDecreaseRate(decreasedList)
                    .ConfigureAwait(false)).ToList();
                var advertisedList = (await SelectToAdvertisementCampaign(decreasedList)
                    .ConfigureAwait(false)).ToList();
                var cancelledList = (await EndProductStocking(decreasedList).ConfigureAwait(false))
                    .ToList();

                foreach (var taskProduct in decreasedList)
                {
                    taskProduct.Price = newPriceList.First(p => p.Id == taskProduct.Id).Price;
                    taskProduct.IsAdvertised =
                        advertisedList.First(p => p.Id == taskProduct.Id).IsAdvertised;
                }

                var notOnlyCancelled = decreasedList.Where(s => !cancelledList.Any(c => c.Id == s.Id)).ToList();
                return notOnlyCancelled;
            }

            _logger.LogDecisionBlock(BusinessTasks.DecreasedSaleOfAtLeastOneProduct, "Nie");
            var filtered = decreasedList.Where(d => d.IsCancelled != true).ToList();
            return filtered;
        }

        private async Task NotifyMarketing()
        {
            _logger.LogInformation(BusinessTasks.NotifyMarketing);
            await _notificationService.NotifyUserTaskCreatedAsync().ConfigureAwait(false);
            await Task.CompletedTask;
        }

        private async Task<IEnumerable<TaskProduct>> EvaluatePriceDecreaseRate(
            IEnumerable<TaskProduct> decreasedList)
        {
            var date = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);
            _logger.LogBusinessCase(BusinessTasks.SaleDecreaseEvaluationTask);
            _logger.LogInput(BusinessTasks.SaleDecreaseEvaluationTask,
                "Produkty so znizenou predajnostou", decreasedList);
            _logger.LogInput(BusinessTasks.SaleDecreaseEvaluationTask, "Dnešný dátum", date);
            var output = await ExecuteUserTask(UserTaskType.PriceUpdate, decreasedList)
                .ConfigureAwait(false);
            _logger.LogOutput(BusinessTasks.SaleDecreaseEvaluationTask,
                "Zoznam produktov so zníženou predajnosťou a zmenenými cenami", output);
            return output;
        }

        private async Task<IEnumerable<TaskProduct>> SelectToAdvertisementCampaign(
            IEnumerable<TaskProduct> updatedList)
        {
            var date = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);
            _logger.LogBusinessCase(BusinessTasks.SelectToAdTask);
            _logger.LogInput(BusinessTasks.SelectToAdTask,
                "Produkty so znizenou predajnostou", updatedList);
            var output = await ExecuteUserTask(UserTaskType.AdvertisementPicking, updatedList)
                .ConfigureAwait(false);
            var onlyAdvertised = output.Where(o => o.IsAdvertised);
            _logger.LogOutput(BusinessTasks.SelectToAdTask,
                "Zoznam produktov zaradenych do reklamnych letakov",
                onlyAdvertised);
            using var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            var adRepository = provider.GetRequiredService<AdvertisedRepository>();
            foreach (var taskProduct in onlyAdvertised)
            {
                
                await adRepository.CreateAsync(new AdvertisedProductEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = taskProduct.Product.Id,
                    ProductName = taskProduct.Product.Name,
                    WeekNumber = taskProduct.SalesWeek
                }).ConfigureAwait(false);
            }
            
            return output;
        }

        private async Task<IEnumerable<TaskProduct>> EndProductStocking(
            IEnumerable<TaskProduct> decreasedProducts)
        {
            var date = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);
            _logger.LogBusinessCase(BusinessTasks.EndStockingTask);
            _logger.LogInput(BusinessTasks.EndStockingTask,
                "Zoznam produktov so zníženou predajnosťou", decreasedProducts);
            var output = await ExecuteUserTask(UserTaskType.OrderingCancellation, decreasedProducts)
                .ConfigureAwait(false);
            var cancelled = output.Where(s => s.IsCancelled);

            _logger.LogOutput(BusinessTasks.EndStockingTask,
                "Zoznam produktov na vyradenie z objednávania", cancelled);

            await _supplier.EndProductStocking(cancelled.Select(s => s.Product.Id))
                .ConfigureAwait(false);
            return cancelled;
        }

        private async Task<IEnumerable<TaskProduct>> ExecuteUserTask(string taskName,
            IEnumerable<TaskProduct> products)
        {
            var awaiter = new TaskCompletionSource<IEnumerable<TaskProduct>>();

            async Task TaskFulfilled(ScheduledTaskResult result)
            {
                _logger.LogDevelopment($"Task {result.Name} has been fulfilled");
                awaiter.SetResult(result.Products);
            }

            async Task TaskFailed(ScheduledTask failedTask)
            {
                _logger.LogDevelopment($"Task {failedTask.Name} has failed to be fulfilled");
                failedTask.IsFailed = true;
                var ex = new UserTaskNotFulfilledException(failedTask);
                awaiter.SetException(ex);
                throw ex;
            }

            _logger.LogTrace(
                $"Creating user task with {typeof(TaskListPortTypeClient)}");

            _logger.LogDebug($"Task: {taskName} added to Task List Service");
            var task = new ScheduledTask(Guid.NewGuid(), products, DateTime.Now, taskName);
            task.OnTaskFulfilled += TaskFulfilled;
            task.OnTaskFailed += TaskFailed;
            _logger.LogDevelopment($"Test: Scheduling user evaluation task {task.Id}");
            await _cronScheduler.ScheduleUserTaskTimeoutJob(task).ConfigureAwait(false);
            var taskID = await _taskClient.SendAsync(task);
            _taskCollection.Push(task, taskID);

            _logger.LogDebug($"{nameof(DecreasedSalesHandler)} is waiting either " +
                             $"on user to fulfil task or on timeout");
            // wait on user result or timeout
            var result =  awaiter.Task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    throw t.Exception;
                }
                
                return t.Result; 
            }).Result;
            return result;
        }

        private readonly IOptimizationNotificationService _notificationService;
        private readonly UserTaskCollectionService _taskCollection;
        private readonly ILogger<DecreasedSalesHandler> _logger;
        private readonly CronSchedulerService _cronScheduler;
        private readonly WsdlCalendarService _calendar;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SupplierService _supplier;
        private readonly ITaskClient _taskClient;
    }

    public class UserTaskNotFulfilledException : Exception
    {
        public UserTaskNotFulfilledException(ScheduledTask task)
        {
            Task = task;
        }

        public override string Message =>
            $"Task {Task.Name} has not been resolved by user in given time";

        public ScheduledTask Task { get; }
    }
}