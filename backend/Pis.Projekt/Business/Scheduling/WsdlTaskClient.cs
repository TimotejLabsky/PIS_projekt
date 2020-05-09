using FiitTaskList;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pis.Projekt.Framework;
using Pis.Projekt.System;
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

        public async Task SendAsync(ScheduledTask scheduledTask)
        {
            var serializedTask = JsonConvert.SerializeObject(scheduledTask, Formatting.Indented);
            _logger.LogDebug($"Sending scheduled task to Task List {serializedTask}");
#if DEBUG
            _logger.LogDevelopment("Task sent to WsdlTaskService. " +
                                   $"Name: {scheduledTask.Name}, " +
                                   $"ScheduledOn: {scheduledTask.ScheduledOn}", scheduledTask);
            await Task.CompletedTask;
#else
            // await _client.createTaskAsync(_configuration.TeamId, _configuration.Password,
            //     nameof(WsdlTaskClient), true, scheduledTask.Name, serializedTask,
            //     // TODO: this probably should be due date
            //     scheduledTask.ScheduledOn);
#endif
        }


        // ReSharper disable once NotAccessedField.Local - Used for production
        private readonly WsdlConfiguration<WsdlTaskClient> _configuration;

        // ReSharper disable once NotAccessedField.Local - Used for production
        private readonly TaskListPortTypeClient _client;
        private readonly ILogger<WsdlTaskClient> _logger;
    }
}