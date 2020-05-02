using FiitTaskList;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace Pis.Projekt.Business.Scheduling
{
    public class WsdlTaskClient : ITaskClient
    {
        public WsdlTaskClient(WsdlTaskClientConfiguration configuration, ILogger<WsdlTaskClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _client = new TaskListPortTypeClient();
        }

        public async Task SendAsync(ScheduledTask scheduledTask)
        {
            var serializedTask = JsonConvert.SerializeObject(scheduledTask);
            _logger.LogDebug($"Sending scheduled task {serializedTask}");
            await _client.createTaskAsync(_configuration.TeamId, _configuration.Password,
                nameof(WsdlTaskClient), true, scheduledTask.Name, serializedTask,
                // TODO: this probably should be due date
                scheduledTask.ScheduledOn);
        }


        private readonly WsdlTaskClientConfiguration _configuration;
        private readonly TaskListPortTypeClient _client;
        private readonly ILogger<WsdlTaskClient> _logger;
    }
}