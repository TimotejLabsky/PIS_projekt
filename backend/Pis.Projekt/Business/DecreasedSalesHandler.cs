using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework;
using Pis.Projekt.System;

// ReSharper disable PossibleMultipleEnumeration

namespace Pis.Projekt.Business
{
    public class DecreasedSalesHandler
    {
        public DecreasedSalesHandler(SupplierService supplier,
            WaiterService waiter,
            UserTaskCollectionService taskCollection,
            ITaskClient taskClient,
            CronSchedulerService cronScheduler,
            ILogger<DecreasedSalesHandler> logger,
            IOptions<WsdlConfiguration<WsdlTaskClient>> wsdlConfiguration)
        {
            _supplier = supplier;
            _waiter = waiter;
            _taskCollection = taskCollection;
            _taskClient = taskClient;
            _cronScheduler = cronScheduler;
            _logger = logger;
            _wsdlConfiguration = wsdlConfiguration.Value;
        }

        public async Task<IEnumerable<PricedProduct>> Handle(
            IEnumerable<PricedProduct> decreasedList)
        {
            if (!decreasedList.Any())
            {
                await NotifyMarketing().ConfigureAwait(false);
                var updatedList =
                    await EvaluatePriceDecreaseRate(decreasedList)
                        .ConfigureAwait(false);
                await SelectToAdvertisementCampaign(updatedList).ConfigureAwait(false);
                await EndProductStocking(updatedList.Select(s => s.Id))
                    .ConfigureAwait(false);
                return updatedList;
            }

            return decreasedList;
        }


        private async Task NotifyMarketing()
        {
            _logger.LogInformation(BusinessTasks.NotifyMarketing);
            // TODO
            await Task.CompletedTask;
        }

        private async Task<IEnumerable<PricedProduct>> EvaluatePriceDecreaseRate(
            IEnumerable<PricedProduct> decreasedList)
        {
            return await ExecuteUserTask("evaluate-sale-rate", decreasedList)
                .ConfigureAwait(false);
        }

        private async Task SelectToAdvertisementCampaign(IEnumerable<PricedProduct> updatedList)
        {
            // var advertisedProducts = await ExecuteUserTask("select-to-ad", updatedList)
                // .ConfigureAwait(false);
        }

        /// <param name="endProductsGuids">List of product guids that should not be ordered anymore</param>
        /// <returns>List od products to be removed from supply order</returns>
        private async Task EndProductStocking(IEnumerable<Guid> endProductsGuids)
        {
            await _supplier.EndProductStocking(endProductsGuids).ConfigureAwait(false);
        }

        private async Task<IEnumerable<PricedProduct>> ExecuteUserTask(string taskName,
            IEnumerable<PricedProduct> products)
        {
            var awaiter = new TaskCompletionSource<IEnumerable<PricedProduct>>();

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
                $"Creating user task with {typeof(FiitTaskList.TaskListPortTypeClient)}");

#if DEBUG
            _logger.LogDevelopment($"Task: {taskName} added to Task List Service");
            var client = new FiitTaskList.TaskListPortTypeClient();
            var response = await client.createTaskAsync(_wsdlConfiguration.TeamId,
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

        private readonly WsdlConfiguration<WsdlTaskClient> _wsdlConfiguration;
        private readonly UserTaskCollectionService _taskCollection;
        private readonly ILogger<DecreasedSalesHandler> _logger;
        private readonly CronSchedulerService _cronScheduler;
        private readonly SupplierService _supplier;
        private readonly ITaskClient _taskClient;
        private readonly WaiterService _waiter;
    }

    public class SupplierService
    {
        public SupplierService(ILogger<SupplierService> logger)
        {
            _logger = logger;
        }

        public Task EndProductStocking(IEnumerable<Guid> endProductsGuids)
        {
            _logger.LogInformation($"Stocking of products {endProductsGuids} has ended");
            return Task.CompletedTask;
        }
        
        public void SendOrder(ProductOrder order)
        {
            _logger.LogInformation($"Supplier received order of ptoducts: {order}");
            return Task.CompletedTask;
        }

        private readonly ILogger<SupplierService> _logger;
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