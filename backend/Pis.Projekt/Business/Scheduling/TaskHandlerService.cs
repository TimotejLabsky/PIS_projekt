using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework;
using Pis.Projekt.System;

namespace Pis.Projekt.Business.Scheduling
{
    // Done
    public class TaskHandlerService
    {
        public TaskHandlerService(PriceCalculatorService priceCalculator,
            CronSchedulerService cronScheduler,
            ITaskClient taskClient,
            ILogger<TaskHandlerService> logger,
            IOptions<WsdlConfiguration<TaskHandlerService>> wsdlConfiguration,
            UserTaskCollectionService taskCollection)
        {
            _priceCalculator = priceCalculator;
            _cronScheduler = cronScheduler;
            _taskClient = taskClient;
            _logger = logger;
            _taskCollection = taskCollection;
            _wsdlConfiguration = wsdlConfiguration.Value;
        }

        public async Task<IEnumerable<PricedProduct>> StartDecreasedSalesTask(
            IEnumerable<PricedProduct> products)
        {
            _logger.LogBusinessCase(BusinessTasks.DecreasedSalesBranch);
            var task = new ProductSalesDecreasedTask(Guid.NewGuid(),
                "product-sales-decreased", products, DateTime.Now);
            await ProcessAsync(task, task.GetType(), default).ConfigureAwait(false);
            var result = task.Result;
            _logger.LogOutput(BusinessTasks.DecreasedSalesBranch,
                "pole aggregatov kde sa predaj znizil o viac ako 20%", result);
            return result;
        }

        public async Task<IEnumerable<PricedProduct>> StartIncreasedSalesTask(
            IEnumerable<PricedProduct> products)
        {
            _logger.LogBusinessCase(BusinessTasks.IncreasedSalesBranch);
            var task = new ProductSalesIncreasedTask("product-sales-increased", products);
            await ProcessAsync(task, task.GetType(), default).ConfigureAwait(false);
            var result = task.Result;
            _logger.LogOutput(BusinessTasks.DecreasedSalesBranch,
                "pole aggregatov kde sa predaj zvisil o viac ako 20%", result);
            return result;
        }

        public async Task ProcessAsync(ITask<IEnumerable<PricedProduct>> task,
            Type type,
            CancellationToken token)
        {
            if (type == typeof(ProductSalesIncreasedTask))
            {
                _logger.LogInformation("Handling products with increased sales");
                task.Result =
                    await HandleProductSalesIncreasedTask((ProductSalesIncreasedTask) task)
                        .ConfigureAwait(false);
            }
            else if (type == typeof(ProductSalesDecreasedTask))
            {
                _logger.LogInformation("Handling products with decreased sales");
                task.Result =
                    await HandleProductSalesDecreasedTask((ProductSalesDecreasedTask) task, token)
                        .ConfigureAwait(false);
            }
            else
            {
                throw new ArgumentException($"Unsupported type of Scheduled type of: {type}");
            }
        }

        public async Task<IEnumerable<PricedProduct>> HandleProductSalesIncreasedTask(
            ProductSalesIncreasedTask task)
        {
            foreach (var product in task.Products)
            {
                product.SalesWeek++;
                product.Price = _priceCalculator.CalculatePrice(product);
            }

            await Task.CompletedTask;
            return task.Products;
        }

        public async Task<IEnumerable<PricedProduct>> HandleProductSalesDecreasedTask(
            ProductSalesDecreasedTask task,
            CancellationToken token)
        {
            var awaiter = new TaskCompletionSource<IEnumerable<PricedProduct>>();

            async Task TaskFulfilled(ScheduledTaskResult result)
            {
                // await _taskRepository.SetResolved(failedTask.Id);
                _logger.LogDevelopment($"Task {result.Name} has been fulfilled");
                awaiter.SetResult(result.Products);
            }

            async Task TaskFailed(ScheduledTask failedTask)
            {
                // await _taskRepository.SetFailedAsync(failedTask.Id);
                _logger.LogDevelopment($"Task {failedTask.Name} has failed to be fulfilled");
                awaiter.SetException(new UserTaskNotFulfilledException(failedTask));
            }

            _logger.LogTrace(
                $"Creating user task with {typeof(FiitTaskList.TaskListPortTypeClient)}");

#if DEBUG
            _logger.LogDevelopment($"Task: {task.Name} added to Task List Service");
#else
            // var client = new FiitTaskList.TaskListPortTypeClient();
            // var response = await client.createTaskAsync(_wsdlConfiguration.TeamId,
            //     _wsdlConfiguration.Password,
            //     "", true,
            //     nameof(ProductSalesDecreasedTask), "descr", DateTime.Now);
#endif
            task.OnTaskFulfilled += TaskFulfilled;
            task.OnTaskFailed += TaskFailed;
            _logger.LogDevelopment($"Test: Scheduling user evaluation task {task.Id}");
            await _cronScheduler.ScheduleUserEvaluationTask(task, token).ConfigureAwait(false);
            // TODO missing transaction ?? maybe
            _taskCollection.Register(task);
            await _taskClient.SendAsync(task);
            return await awaiter.Task.ConfigureAwait(false);
        }

        private readonly ITaskClient _taskClient;
        private readonly ILogger<TaskHandlerService> _logger;
        private readonly CronSchedulerService _cronScheduler;
        private readonly PriceCalculatorService _priceCalculator;
        private readonly UserTaskCollectionService _taskCollection;
        private readonly WsdlConfiguration<TaskHandlerService> _wsdlConfiguration;
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