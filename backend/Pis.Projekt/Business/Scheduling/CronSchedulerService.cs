// #nullable enable
// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Hosting;
// using Pis.Projekt.Business.Scheduling.Impl;
// using Quartz;
//
// namespace Pis.Projekt.Business.Scheduling
// {
//     public class CronSchedulerService : IHostedService
//     {
//         public CronSchedulerService(CronSchedulerConfiguration configuration,
//             ISchedulerFactory factory,
//             OptimizationJobFactory jobFactory)
//         {
//             _configuration = configuration;
//             _factory = factory;
//             _jobFactory = jobFactory;
//         }
//
//
//         public async Task StartAsync(CancellationToken cancellationToken)
//         {
//             _scheduler = await _factory.GetScheduler(cancellationToken);
//             _scheduler.JobFactory = _jobFactory;
//
//             await _scheduler.Start(cancellationToken);
//         }
//
//         public async Task StopAsync(CancellationToken cancellationToken)
//         {
//             if (_scheduler != null)
//             {
//                 await _scheduler.Shutdown(cancellationToken).ConfigureAwait(false);
//             }
//         }
//
//         public async Task<DateTime> ScheduleNextOptimalizationTask(CancellationToken token)
//         {
//             var job = _jobFactory.CreateOptimizationJob();
//             var trigger = CreateTrigger<OptimizationJob>(_configuration);
//             await _scheduler.ScheduleJob(job, trigger, token)
//                 .ConfigureAwait(false);
//             return DateTime.Now; // TODO return correct datetime of next optimization
//         }
//
//         public async Task ScheduleUserEvaluationTask(ProductSalesDecreasedTask task,
//             CancellationToken token)
//         {
//             // create job from scheduled task
//             var job = _jobFactory.CreateProductSalesDecreasedTimeoutJob(task);
//             var trigger = CreateTrigger<UserEvaluationJob>(_configuration);
//             await _scheduler.ScheduleJob(job, trigger, token
//             ).ConfigureAwait(false);
//         }
//
//         private static IJobDetail CreateJob<TJob>(CronSchedulerConfiguration schedule)
//             where TJob : IJob
//         {
//             var jobType = typeof(TJob);
//             return JobBuilder
//                 .Create<TJob>()
//                 .WithIdentity(jobType.FullName)
//                 .WithDescription(jobType.Name)
//                 .Build();
//         }
//
//         private static ITrigger CreateTrigger<TJob>(CronSchedulerConfiguration schedule)
//             where TJob : IJob
//         {
//             return TriggerBuilder
//                 .Create()
//                 .WithIdentity($"{schedule.Name}.trigger")
//                 .WithCronSchedule(schedule.CronExpression.ToString())
//                 .WithDescription(schedule.CronExpression.ToString())
//                 .Build();
//         }
//
//         private IScheduler? _scheduler;
//         private readonly ISchedulerFactory _factory;
//         private readonly OptimizationJobFactory _jobFactory;
//         private readonly CronSchedulerConfiguration _configuration;
//
//         public class CronSchedulerConfiguration
//         {
//             public string Name { get; set; }
//             public CronExpression CronExpression { get; set; }
//         }
//     }
// }