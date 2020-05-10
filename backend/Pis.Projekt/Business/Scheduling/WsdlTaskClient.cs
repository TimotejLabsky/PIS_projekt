using System;
using System.Globalization;
using System.Threading.Tasks;
using FiitTaskList;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pis.Projekt.Framework;
using Task = System.Threading.Tasks.Task;

namespace Pis.Projekt.Business.Scheduling
{
    // ReSharper disable once ClassNeverInstantiated.Global - Called in DI
    public class WsdlTaskClient : ITaskClient
    {
        public WsdlTaskClient(IOptions<WsdlConfiguration<WsdlTaskClient>> configuration,
            ILogger<WsdlTaskClient> logger)
        {
            _configuration = configuration.Value;
            _client = new TaskListPortTypeClient();
            _logger = logger;
        }

        public async Task<int> SendAsync(ScheduledTask scheduledTask)
        {
            var serializedTask = JsonConvert.SerializeObject(scheduledTask, Formatting.Indented);
            _logger.LogDebug($"Sending scheduled task to Task List {serializedTask}");
            var response = await _client.createTaskAsync(_configuration.TeamId,
                _configuration.Password,
                nameof(WsdlTaskClient), true, scheduledTask.Name, serializedTask,
                DateTime.ParseExact(scheduledTask.ScheduledOn.ToString("yyyy-MM-dd HH:mm:ss"),
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            return response.task_id;
        }

        // ReSharper disable once NotAccessedField.Local - Used for production
        private readonly WsdlConfiguration<WsdlTaskClient> _configuration;

        // ReSharper disable once NotAccessedField.Local - Used for production
        private readonly TaskListPortTypeClient _client;
        private readonly ILogger<WsdlTaskClient> _logger;

        public async Task SetCompleteAsync(int id)
        {
            await _client.setCompletenessAsync(id, _configuration.TeamId, _configuration.Password,
                1).ConfigureAwait(false);
        }
    }
}