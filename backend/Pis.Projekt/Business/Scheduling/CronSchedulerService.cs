#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Pis.Projekt.Business.Scheduling.Impl;
using Quartz;
using Quartz.Spi;

namespace Pis.Projekt.Business.Scheduling
{
    public class CronSchedulerService : IHostedService
    {
        public CronSchedulerService(CronSchedulerConfiguration configuration,
            ISchedulerFactory factory,
            WeeklyJobFactory jobFactory)
        {
            _configuration = configuration;
            _factory = factory;
            _jobFactory = jobFactory;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _factory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _fac;

            await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            
            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown(cancellationToken);
        }

        public async Task PlanNextOptimalization()
        {
            var job = _jobFactory.NewJob(, );
            await _scheduler.ScheduleJob(job)
            return Task.CompletedTask;
        }
        
        public void ScheduleUserEvaluationTask(ProductSalesDecreasedTask task)
        {
            // create job from scheduled task
            var job = _jobFactory.NewJob(, );
            _scheduler.ScheduleJob(job);
        }
        
        private static IJobDetail CreateJob<TJob>(CronSchedulerConfiguration schedule)
        {
            var jobType = typeof(TJob);
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger<TJob>(CronSchedulerConfiguration schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.Name}.trigger")
                .WithCronSchedule(schedule.Expression)
                .WithDescription(schedule.Expression)
                .Build();
        }

        private IScheduler? _scheduler;
        private readonly ISchedulerFactory _factory;
        private readonly WeeklyJobFactory _jobFactory;
        private readonly CronSchedulerConfiguration _configuration;

        public class CronSchedulerConfiguration
        {
            public string Name { get; set; }
            public string Expression { get; set; }

            public IJob RepeatedAction { get; set; }
        }
    }
}