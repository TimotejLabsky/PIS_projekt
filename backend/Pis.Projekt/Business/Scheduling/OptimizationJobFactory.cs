using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Scheduling.Impl;
using Quartz;
using Quartz.Spi;

namespace Pis.Projekt.Business.Scheduling
{
    // Done
    public class OptimizationJobFactory : IJobFactory
    {
        public OptimizationJobFactory(IServiceProvider provider, ILogger<OptimizationJobFactory> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            _logger.LogDebug($"Creating job of type: {bundle.JobDetail.JobType}, " +
                             $"Name: {bundle.JobDetail.Description}");
            var job = _provider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            _logger.LogError("here");
            return job;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }

        private readonly ILogger<OptimizationJobFactory> _logger;
        private readonly IServiceProvider _provider;
    }
}