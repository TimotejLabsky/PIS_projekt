using FiitTaskList;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace Pis.Projekt.Business.Scheduling
{
    public class WsdlTaskClient : ITaskClient
    {
        public WsdlTaskClient(WsdlTaskClientConfiguration configuration)
        {
            _configuration = configuration;
            _client = new TaskListPortTypeClient();
        }

        public async Task SendAsync(ScheduledTask scheduledTask)
        {
            var serializedTask = JsonConvert.SerializeObject(scheduledTask);

            await _client.createTaskAsync(_configuration.TeamId, _configuration.Password,
                nameof(WsdlTaskClient), true, scheduledTask.Name, serializedTask,
                // TODO: this probably should be due date
                scheduledTask.ScheduledOn);
        }


        private readonly WsdlTaskClientConfiguration _configuration;
        private readonly TaskListPortTypeClient _client;
    }
}