using System;
using Quartz;

namespace Pis.Projekt.Business.Scheduling
{
    public class JobSchedule<TJob> where TJob: IJob
    {
        public JobSchedule(string cronExpression)
        {
            CronExpression = cronExpression;
        }

        public Type JobType => typeof(TJob);
        public string CronExpression { get; }
        
    }
}