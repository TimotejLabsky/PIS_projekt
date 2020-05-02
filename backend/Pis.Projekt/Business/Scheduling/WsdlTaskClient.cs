using FiitTaskList;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pis.Projekt.Framework;
using Task = System.Threading.Tasks.Task;

namespace Pis.Projekt.Business.Scheduling
{
    public class WsdlTaskClient : ITaskClient
    {

        public WsdlTaskClient(IOptions<WsdlConfiguration<WsdlTaskClient>> configuration,ILogger<WsdlTaskClient> logger)
        {
            _configuration = configuration.Value;
            _client = new TaskListPortTypeClient();
            _logger = logger;

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


        private readonly WsdlConfiguration<WsdlTaskClient> _configuration;
        private readonly TaskListPortTypeClient _client;
        private readonly ILogger<WsdlTaskClient> _logger;
    }
}