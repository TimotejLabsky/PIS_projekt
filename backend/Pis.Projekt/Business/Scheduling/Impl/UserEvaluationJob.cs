using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;
using Quartz;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class UserEvaluationJob : IJob
    {
        public UserEvaluationJob()
        {
            // _task = ;
            Console.WriteLine("Job created!!!!!!!!!!!!!");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Evaluate deadline
            // _task.Evaluate();
            Console.WriteLine("Job executed!!!!!!!!!!!!!");
            return Task.CompletedTask;
        }

        // private readonly ScheduledTask _task;
    }
}