using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pis.Projekt.Business.Scheduling;

namespace Pis.Projekt.Business
{
    public class UserTaskCollectionService
    {
        public UserTaskCollectionService(ILogger<UserTaskCollectionService> logger)
        {
            _logger = logger;
            _scheduledTasks = new Queue<ScheduledTask>();
        }

        public void Push(ScheduledTask task)
        {
            var taskJson = JsonConvert.SerializeObject(task, Formatting.Indented);
            _logger.LogDebug($"Storing new user task\n{taskJson}");
            _scheduledTasks.Enqueue(task);
            var tasksJson =
                JsonConvert.SerializeObject(_scheduledTasks.ToList(), Formatting.Indented);
            _logger.LogDebug(
                $"Task {task.Id}: {task.Name} stored\nCurrent stored tasks:\n{tasksJson}");
        }

        public ScheduledTask Find(Guid id)
        {
            return _scheduledTasks.Where(s => s.Id == id).First();
        }

        public ScheduledTask GetNext()
        {
            return _scheduledTasks.Peek();
        }

        public IEnumerable<ScheduledTask> All()
        {
            return _scheduledTasks;
        }

        private readonly Queue<ScheduledTask> _scheduledTasks;
        private readonly ILogger<UserTaskCollectionService> _logger;
    }
}