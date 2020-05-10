#nullable enable
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Scheduling.Impl;
using Quartz;

namespace Pis.Projekt.Business.Scheduling
{
    public class CronSchedulerService : IHostedService
    {
        public CronSchedulerService(IOptions<CronSchedulerConfiguration> configuration,
            ISchedulerFactory factory,
            OptimizationJobFactory jobFactory,
            ILogger<CronSchedulerService> logger)
        {
            _configuration = configuration.Value;
            _factory = factory;
            _jobFactory = jobFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _factory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;
            _logger.LogDebug("Starting scheduler...");
            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_scheduler != null)
            {
                _logger.LogDebug("Stopping scheduler");
                await _scheduler.Shutdown(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<DateTime> ScheduleOptimalizationTask(CancellationToken token =
            default)
        {
            var job = CreateJob<OptimizationJob>("optimalization-job",
                "Weekly optimalization of product prices");
            var trigger = CreateTrigger<OptimizationJob>(job, _configuration);
            var date = await _scheduler.ScheduleJob(job, trigger, token);
            _logger.LogBusinessCase(BusinessTasks.OptimizationSchedulingTask,
                $"Task: {job.Description} scheduled on {date}");
            return DateTime.Now;
        }

        public async Task ScheduleUserTaskTimeoutJob(ScheduledTask task)
        {
            // create job from scheduled task
            var job = CreateJob<UserTaskTimeoutEvaluationJob>("user-eval",
                "Waiting on user Evaluation or timeout");
            var trigger = CreateOneTimeTrigger<UserTaskTimeoutEvaluationJob>(job, _configuration);
            job.JobDataMap.Add("task", task);
            var date = await _scheduler.ScheduleJob(job, trigger).ConfigureAwait(false);
            _logger.LogBusinessCase(BusinessTasks.UserEvaluationSchedulingTask,
                $"Task: {job.Description} scheduled on {date}");
        }

        private static IJobDetail CreateJob<TJob>(string name, string description)
            where TJob : IJob
        {
            var jobType = typeof(TJob);
            return JobBuilder
                .Create<TJob>()
                .WithIdentity(name)
                .WithDescription(description)
                .Build();
        }

        private static ITrigger CreateOneTimeTrigger<TJob>(IJobDetail job,
            CronSchedulerConfiguration schedule)
            where TJob : IJob
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{job.Key.Name}.trigger")
                .ForJob(job)
                .WithSimpleSchedule(a => a.WithRepeatCount(2)
                    .WithInterval(schedule.UserTaskTimeout))
                .Build();
        }

        private static ITrigger CreateTrigger<TJob>(IJobDetail jobDetail,
            CronSchedulerConfiguration schedule)
            where TJob : IJob
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.Name}.trigger")
                .WithCronSchedule(schedule.CronExpressionString)
                .WithDescription(schedule.CronExpressionString)
                .ForJob(jobDetail)
                .Build();
        }

        private IScheduler _scheduler;
        private readonly ISchedulerFactory _factory;
        private readonly OptimizationJobFactory _jobFactory;
        private readonly ILogger<CronSchedulerService> _logger;
        private readonly CronSchedulerConfiguration _configuration;

        public class CronSchedulerConfiguration
        {
            [ConfigurationProperty("Name", IsRequired = true)]
            public string Name { get; set; }

            [ConfigurationProperty("CronExpressionString", IsRequired = true)]
            public string CronExpressionString { get; set; }

            [ConfigurationProperty("CronExpression", IsRequired = false)]
            public CronExpression CronExpression => new CronExpression(CronExpressionString);

            public TimeSpan UserTaskTimeout { get; set; }
        }
    }
}