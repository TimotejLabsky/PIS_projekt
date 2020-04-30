using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Business.Scheduling
{
    // Done
    public class TaskSchedulerService : IHostedService
    {
        public TaskSchedulerService(PriceCalculatorService priceCalculator,
            CronSchedulerService cronScheduler,
            IScheduledTaskRepository taskRepository,
            IMapper mapper,
            ITaskClient taskClient)
        {
            _priceCalculator = priceCalculator;
            _cronScheduler = cronScheduler;
            _taskRepository = taskRepository;
            _mapper = mapper;
            _taskClient = taskClient;
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

        public async Task ProcessAsync(ITask<IEnumerable<PricedProduct>> task, Type type)
        {
            if (type == typeof(ProductSalesIncreasedTask))
            {
                task.Result =
                    await HandleProductSalesIncreasedTask((ProductSalesIncreasedTask) task)
                        .ConfigureAwait(false);
            }
            else if (type == typeof(ProductSalesIncreasedTask))
            {
                task.Result =
                    await HandleProductSalesDecreasedTask((ProductSalesDecreasedTask) task)
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
            ProductSalesDecreasedTask task)
        {
            var awaiter = new TaskCompletionSource<IEnumerable<PricedProduct>>();

            async Task TaskFulfilled(ScheduledTaskResult result)
            {
                await _taskRepository.SetResolvedAsync(result.TaskId);
                awaiter.SetResult(result.Products);
            }

            async Task TaskFailed(ScheduledTask failedTask)
            {
                await _taskRepository.SetFailedAsync(failedTask.Id);
                awaiter.SetException(new UserTaskNotFulfilledException(failedTask));
            }

            var client = new FiitTaskList.TaskListPortTypeClient();
            var response = await client.createTaskAsync("017", "root",
                "", true,
                nameof(ProductSalesDecreasedTask), "descr", DateTime.Now);
            task.OnTaskFulfilled += TaskFulfilled;
            task.OnTaskFailed += TaskFailed;
            _cronScheduler.ScheduleUserEvaluationTask(task);
            
            var scheduledTask = task.Schedule(response.task_id);
            // TODO missing transaction
            await _taskRepository.CreateAsync(scheduledTask).ConfigureAwait(false);
            await _taskClient.SendAsync(scheduledTask);
            return await awaiter.Task.ConfigureAwait(false);
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_task != null)
            {
                await ProcessAsync(_task, _task.GetType()).ConfigureAwait(false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => _task = null, cancellationToken);
        }

        private readonly PriceCalculatorService _priceCalculator;


        private readonly IMapper _mapper;
        private readonly ITaskClient _taskClient;
        private ITask<IEnumerable<PricedProduct>> _task;
        private readonly CronSchedulerService _cronScheduler;
        private readonly IScheduledTaskRepository _taskRepository;
    }

    public class UserTaskNotFulfilledException : Exception
    {
        public UserTaskNotFulfilledException(ScheduledTask task)
        {
            this.task = task;
        }

        public override string Message =>
            $"Task {task.Id}:{task.Name} has not been resolved by user in given time";


        public ScheduledTask task { get; }
    }
}