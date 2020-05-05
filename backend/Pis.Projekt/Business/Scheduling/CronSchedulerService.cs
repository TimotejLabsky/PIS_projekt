#nullable enable
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Scheduling.Impl;
using Pis.Projekt.System;
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

        public async Task<DateTime> ScheduleNextOptimalizationTask(CancellationToken token =
            default)
        {
            var job = CreateJob<OptimizationJob>(_configuration);
            var trigger = CreateTrigger<OptimizationJob>(job, _configuration);
            var date = await _scheduler.ScheduleJob(job, trigger, token);
            _logger.LogDevelopment(
                $"{nameof(OptimizationJob)} scheduled and returned date: {date}");
            return DateTime.Now;
        }

        public async Task ScheduleUserEvaluationTask(ProductSalesDecreasedTask task,
            CancellationToken token)
        {
            // create job from scheduled task
            var job = CreateJob<UserEvaluationJob>(_configuration);
            var trigger = CreateTrigger<UserEvaluationJob>(job, _configuration);
            var date = await _scheduler.ScheduleJob(job, trigger, token);
            
            _logger.LogDevelopment(
                $"{nameof(UserEvaluationJob)} scheduled and returned date: {date}");
        }

        private static IJobDetail CreateJob<TJob>(CronSchedulerConfiguration schedule)
            where TJob : IJob
        {
            var jobType = typeof(TJob);
            return JobBuilder
                .Create<TJob>()
                .WithIdentity(schedule.Name)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger<TJob>(IJobDetail jobDetail, CronSchedulerConfiguration schedule)
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

        private IScheduler? _scheduler;
        private readonly ISchedulerFactory _factory;
        private readonly OptimizationJobFactory _jobFactory;
        private readonly CronSchedulerConfiguration _configuration;
        private readonly ILogger<CronSchedulerService> _logger;

        public class CronSchedulerConfiguration
        {
            [ConfigurationProperty("Name", IsRequired = true)]
            public string Name { get; set; }

            [ConfigurationProperty("CronExpressionString", IsRequired = true)]
            public string CronExpressionString { get; set; }

            [ConfigurationProperty("CronExpression", IsRequired = false)]
            public CronExpression CronExpression => new CronExpression(CronExpressionString);
        }
    }
}