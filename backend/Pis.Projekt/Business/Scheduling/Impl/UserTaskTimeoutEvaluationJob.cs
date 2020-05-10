using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class UserTaskTimeoutEvaluationJob : IJob
    {
        public UserTaskTimeoutEvaluationJob(ILogger<UserTaskTimeoutEvaluationJob> logger)
        {
            _logger = logger;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            var task = (ScheduledTask)context.JobDetail.JobDataMap["task"];
            _logger.LogInformation($"Evaluating timeout of task {task.Id}: {task.Name}");

            // Evaluate deadline
            task.Evaluate();
            context.Scheduler.UnscheduleJob(context.Trigger.Key);
            return Task.CompletedTask;
        }

        private readonly ILogger<UserTaskTimeoutEvaluationJob> _logger;
    }
}