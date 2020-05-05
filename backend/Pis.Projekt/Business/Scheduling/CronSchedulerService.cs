#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Scheduling.Impl;
using Quartz;

namespace Pis.Projekt.Business.Scheduling
{
    public class CronSchedulerService
    {
        public CronSchedulerService(IOptions<CronSchedulerConfiguration> configuration,
            ISchedulerFactory factory,
            OptimizationJobFactory jobFactory, ILogger<CronSchedulerService> logger)
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
            _logger.LogDebug("Starting scheduler");
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

        public async Task<DateTime> ScheduleNextOptimalizationTask(CancellationToken token)
        {
            var job = CreateJob<OptimizationJob>(_configuration);
            var trigger = CreateTrigger<OptimizationJob>(_configuration);
            var date = await _scheduler.ScheduleJob(job, trigger, token);
            return DateTime.Now;
        }

        public async Task ScheduleUserEvaluationTask(ProductSalesDecreasedTask task,
            CancellationToken token)
        {
            // create job from scheduled task
            var job = CreateJob<OptimizationJob>(_configuration);
            var trigger = CreateTrigger<OptimizationJob>(_configuration);
            var date = await _scheduler.ScheduleJob(job, trigger, token);
            
        }

        private static IJobDetail CreateJob<TJob>(CronSchedulerConfiguration schedule)
            where TJob : IJob
        {
            var jobType = typeof(TJob);
            return JobBuilder
                .Create<TJob>()
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger<TJob>(CronSchedulerConfiguration schedule)
            where TJob : IJob
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.Name}.trigger")
                .WithCronSchedule(schedule.CronExpression.ToString())
                .WithDescription(schedule.CronExpression.ToString())
                .Build();
        }

        private IScheduler? _scheduler;
        private readonly ISchedulerFactory _factory;
        private readonly OptimizationJobFactory _jobFactory;
        private readonly CronSchedulerConfiguration _configuration;
        private readonly ILogger<CronSchedulerService> _logger;
        public class CronSchedulerConfiguration
        {
            public string Name { get; set; }
            public CronExpression CronExpression { get; set; }
        }
    }
}
