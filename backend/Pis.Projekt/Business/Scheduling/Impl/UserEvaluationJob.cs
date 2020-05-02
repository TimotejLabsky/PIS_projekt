using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;
using Quartz;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class UserEvaluationJob : IJob
    {
        public UserEvaluationJob(ISchedulabletask<UserTask> task)
        {
            _task = task;
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Evaluate deadline
            _task.Evaluate();
            return Task.CompletedTask;
        }

        private readonly ISchedulabletask<UserTask> _task;
    }
}