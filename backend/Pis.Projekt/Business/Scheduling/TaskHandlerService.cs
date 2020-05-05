using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework;
using Pis.Projekt.System;

namespace Pis.Projekt.Business.Scheduling
{
    // Done
    public class TaskHandlerService : IHostedService
    {
        public TaskHandlerService(PriceCalculatorService priceCalculator,
            // CronSchedulerService cronScheduler,
            ITaskClient taskClient,
            ILogger<TaskHandlerService> logger,
            IOptions<WsdlConfiguration<TaskHandlerService>> wsdlConfiguration)
        {
            _priceCalculator = priceCalculator;
            // _cronScheduler = cronScheduler;
            _taskClient = taskClient;
            _logger = logger;
            _wsdlConfiguration = wsdlConfiguration.Value;
        }

        public async Task<IEnumerable<PricedProduct>> StartDecreasedSalesTask(
            IEnumerable<PricedProduct> products)
        {
            _task = new ProductSalesDecreasedTask("product-sales-decreased", products,
                DateTime.Now);
            await StartAsync(default).ConfigureAwait(false);
            return _task.Result;
        }

        public async Task<IEnumerable<PricedProduct>> StartIncreasedSalesTask(
            IEnumerable<PricedProduct> products)
        {
            _task = new ProductSalesIncreasedTask("products increased", products);
            await StartAsync(default).ConfigureAwait(false);
            return _task.Result;
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

            var client = new FiitTaskList.TaskListPortTypeClient();
            _logger.LogTrace($"Creating task with {typeof(FiitTaskList.TaskListPortTypeClient)}");
            var response = await client.createTaskAsync(_wsdlConfiguration.TeamId,
                _wsdlConfiguration.Password,
                "", true,
                nameof(ProductSalesDecreasedTask), "descr", DateTime.Now);
            task.OnTaskFulfilled += TaskFulfilled;
            task.OnTaskFailed += TaskFailed;
            // await _cronScheduler.ScheduleUserEvaluationTask(task, token).ConfigureAwait(false);

            var scheduledTask = task;
            // TODO missing transaction
            // await _taskRepository.CreateAsync(scheduledTask, token)
            // .ConfigureAwait(false);
            await _taskClient.SendAsync(scheduledTask);
            return await awaiter.Task.ConfigureAwait(false);
        }


        public async Task StartAsync(CancellationToken token)
        {
            _logger.LogTrace("Starting Task scheduler");
            if (_task != null)
            {
                await ProcessAsync(_task, _task.GetType(), token).ConfigureAwait(false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Stoping Task scheduler");
            return Task.Run(() => _task = null, cancellationToken);
        }

        private ITask<IEnumerable<PricedProduct>> _task;

        // private readonly CronSchedulerService _cronScheduler;
        private readonly ITaskClient _taskClient;
        private readonly PriceCalculatorService _priceCalculator;
        private readonly ILogger<TaskHandlerService> _logger;
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