using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiitTaskList;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Calendar;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Domain.DTOs;
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
            IOptions<WsdlConfiguration<WsdlTaskClient>> wsdlConfiguration,
            IOptimizationNotificationService notificationService,
            WsdlCalendarService calendar,
            TaskListPortTypeClient wsdlTaskList)
        {
            _supplier = supplier;
            _taskCollection = taskCollection;
            _taskClient = taskClient;
            _cronScheduler = cronScheduler;
            _logger = logger;
            _notificationService = notificationService;
            _calendar = calendar;
            _wsdlTaskList = wsdlTaskList;
            _wsdlConfiguration = wsdlConfiguration.Value;
        }

        public async Task<IEnumerable<PricedProduct>> Handle(
            IEnumerable<TaskProduct> decreasedList)
        {
            _logger.LogBusinessCase(BusinessTasks.DecreasedSalesBranch);
            if (decreasedList.Any())
            {
                _logger.LogDecisionBlock(BusinessTasks.DecreasedSaleOfAtLeastOneProduct, "Ano");
                await NotifyMarketing().ConfigureAwait(false);
                var updatedList =
                    await EvaluatePriceDecreaseRate(decreasedList)
                        .ConfigureAwait(false);
                await SelectToAdvertisementCampaign(updatedList).ConfigureAwait(false);
                await EndProductStocking(updatedList.Select(s => s.Id))
                    .ConfigureAwait(false);
                return updatedList;
            }
            else
            {
                _logger.LogDecisionBlock(BusinessTasks.DecreasedSaleOfAtLeastOneProduct, "Nie");
            }

            return decreasedList;
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

        private async Task SelectToAdvertisementCampaign(IEnumerable<TaskProduct> updatedList)
        {
            var date = await _calendar.GetCurrentDateAsync().ConfigureAwait(false);
            _logger.LogBusinessCase(BusinessTasks.SelectToAdTask);
            _logger.LogInput(BusinessTasks.SelectToAdTask,
                "Produkty so znizenou predajnostou", updatedList);
            _logger.LogInput(BusinessTasks.SelectToAdTask, "Dnešný dátum", date);
            var output = await ExecuteUserTask(UserTaskType.AdvertisementPicking, updatedList)
                .ConfigureAwait(false);
            _logger.LogOutput(BusinessTasks.SelectToAdTask,
                "Zoznam produktov zaradenych do reklamnych letakov", output);
        }

        /// <param name="endProductsGuids">List of product guids that should not be ordered anymore</param>
        /// <returns>List od products to be removed from supply order</returns>
        private async Task EndProductStocking(IEnumerable<Guid> endProductsGuids)
        {
            await _supplier.EndProductStocking(endProductsGuids).ConfigureAwait(false);
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
                awaiter.SetException(new UserTaskNotFulfilledException(failedTask));
            }

            _logger.LogTrace(
                $"Creating user task with {typeof(TaskListPortTypeClient)}");

#if DEBUG
            _logger.LogDevelopment($"Task: {taskName} added to Task List Service");
#else
            //TODO: make this format beautiful
            var response = await _wsdlTaskList.createTaskAsync(_wsdlConfiguration.TeamId,
                _wsdlConfiguration.Password,
                "", true,
                nameof(ProductSalesDecreasedTask), "descr", DateTime.Now);
#endif
            var task = new ScheduledTask(Guid.NewGuid(), products, DateTime.Now, taskName);
            task.OnTaskFulfilled += TaskFulfilled;
            task.OnTaskFailed += TaskFailed;
            _logger.LogDevelopment($"Test: Scheduling user evaluation task {task.Id}");
            await _cronScheduler.ScheduleUserTaskTimeoutJob(task).ConfigureAwait(false);
            _taskCollection.Push(task);
            await _taskClient.SendAsync(task);
            _logger.LogDebug($"{nameof(DecreasedSalesHandler)} is waiting either " +
                             $"on user to fulfil task or on timeout");
            // wait on user result or timeout
            return await awaiter.Task.ConfigureAwait(false);
        }

        private readonly IOptimizationNotificationService _notificationService;
        private readonly WsdlConfiguration<WsdlTaskClient> _wsdlConfiguration;
        private readonly UserTaskCollectionService _taskCollection;
        private readonly ILogger<DecreasedSalesHandler> _logger;
        private readonly TaskListPortTypeClient _wsdlTaskList;
        private readonly CronSchedulerService _cronScheduler;
        private readonly WsdlCalendarService _calendar;
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