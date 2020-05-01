using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Pis.Projekt.Business.Scheduling
{
    // Done
    public class WeeklyJobFactory : IJobFactory
    {
        public WeeklyJobFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job =  _provider.GetRequiredService<WeeklyOptimizationJob>();
            return job;
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }

        private readonly IServiceProvider _provider;
    }
}