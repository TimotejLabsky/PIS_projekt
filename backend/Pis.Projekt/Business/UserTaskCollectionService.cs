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
            _scheduledTasks = new Queue<KeyValuePair<ScheduledTask, int>>();
        }

        public void Push(ScheduledTask task, int taskID)
        {
            var taskJson = JsonConvert.SerializeObject(task, Formatting.Indented);
            _logger.LogDebug($"Storing new user task\n{taskJson}");
            _scheduledTasks.Enqueue(new KeyValuePair<ScheduledTask, int>(task, taskID));
            var tasksJson =
                JsonConvert.SerializeObject(_scheduledTasks.ToList(), Formatting.Indented);
            _logger.LogDebug(
                $"Task {task.Id}: {task.Name} stored\nCurrent stored tasks:\n{tasksJson}");
        }

        public KeyValuePair<ScheduledTask, int> Find(Guid id)
        {
            var result = _scheduledTasks.Where(s => s.Key.Id == id).First();

            if (result.Key.IsFailed)
            {
                _scheduledTasks.Dequeue();
                throw new KeyNotFoundException($"Task with id {id} does not exist");
            }

            return result;
        }

        public void Fulfill(Guid id)
        {
            var deq = _scheduledTasks.Dequeue();
            if (id != deq.Key.Id)
            {
                var taskJson = JsonConvert.SerializeObject(deq, Formatting.Indented);
                throw new InvalidOperationException(
                    $"Unable to dequeue task: {id}. Next task in line was {taskJson}");
            }
        }

        public ScheduledTask GetNext()
        {
            return _scheduledTasks.Peek().Key;
        }

        public IEnumerable<KeyValuePair<ScheduledTask, int>> All()
        {
            return _scheduledTasks;
        }

        private readonly Queue<KeyValuePair<ScheduledTask, int>> _scheduledTasks;
        private readonly ILogger<UserTaskCollectionService> _logger;
    }
}