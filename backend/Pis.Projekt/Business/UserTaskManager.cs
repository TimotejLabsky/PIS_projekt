using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FiitTaskList;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.System;
using Task = System.Threading.Tasks.Task;

namespace Pis.Projekt.Business
{
    public class UserTaskManager
    {
        public UserTaskManager(ILogger<UserTaskManager> logger,
            CronSchedulerService cronScheduler,
            ITaskClient taskClient,
            UserTaskCollectionService taskCollection)
        {
            _logger = logger;
            _cronScheduler = cronScheduler;
            _taskClient = taskClient;
            _taskCollection = taskCollection;
        }

        public async Task<IEnumerable<TaskProduct>> ExecuteUserTask(string taskName,
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
            var result = awaiter.Task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    throw t.Exception;
                }

                return t.Result;
            }).Result;
            return result;
        }

        private readonly UserTaskCollectionService _taskCollection;
        private readonly CronSchedulerService _cronScheduler;
        private readonly ILogger<UserTaskManager> _logger;
        private readonly ITaskClient _taskClient;
    }
}