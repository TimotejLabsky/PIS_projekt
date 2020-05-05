using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.Framework;

namespace Pis.Projekt.Business.Scheduling
{
    // Done
    public class TaskSchedulerService : IHostedService
    {
        public TaskSchedulerService(PriceCalculatorService priceCalculator,
            // CronSchedulerService cronScheduler,
            ITaskClient taskClient, ILogger<TaskSchedulerService> logger, WsdlConfiguration<TaskScheduler> wsdlConfiguration)
        {
            _priceCalculator = priceCalculator;
            // _cronScheduler = cronScheduler;
            _taskClient = taskClient;
            _logger = logger;
            _wsdlConfiguration = wsdlConfiguration;
        }

        public async Task<IEnumerable<PricedProduct>> RegisterDecreasedSalesTask(
            IEnumerable<PricedProduct> products)
        {
            _task = new ProductSalesDecreasedTask("products decreased", products);
            await StartAsync(default).ConfigureAwait(false);
            return _task.Result;
        }

        public async Task<IEnumerable<PricedProduct>> RegisterIncreasedSalesTask(
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
                // await _taskRepository.SetResolvedAsync(result.TaskId);
                awaiter.SetResult(result.Products);
            }

            async Task TaskFailed(ScheduledTask failedTask)
            {
                // await _taskRepository.SetFailedAsync(failedTask.Id);
                awaiter.SetException(new UserTaskNotFulfilledException(failedTask));
            }

            var client = new FiitTaskList.TaskListPortTypeClient();
            // TODO: team_id, psw and creater_name => Take from configuration
            _logger.LogTrace($"Creating task with {typeof(FiitTaskList.TaskListPortTypeClient)}");
            var response = await client.createTaskAsync(_wsdlConfiguration.TeamId, _wsdlConfiguration.Password,
                "", true,
                nameof(ProductSalesDecreasedTask), "descr", DateTime.Now);
            // task.OnTaskFulfilled += TaskFulfilled;
            // task.OnTaskFailed += TaskFailed;
            // await _cronScheduler.ScheduleUserEvaluationTask(task, token).ConfigureAwait(false);
            
            var scheduledTask = task.Schedule(response.task_id);
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

        private readonly PriceCalculatorService _priceCalculator;


        private readonly ITaskClient _taskClient;
        private ITask<IEnumerable<PricedProduct>> _task;
        // private readonly CronSchedulerService _cronScheduler;
        private readonly ILogger<TaskSchedulerService> _logger;
        private readonly WsdlConfiguration<TaskScheduler> _wsdlConfiguration;
    }

    public class UserTaskNotFulfilledException : Exception
    {
        public UserTaskNotFulfilledException(ScheduledTask task)
        {
            Task = task;
        }

        public override string Message =>
            $"Task {Task.Id}:{Task.Name} has not been resolved by user in given time";


        public ScheduledTask Task { get; }
    }
}