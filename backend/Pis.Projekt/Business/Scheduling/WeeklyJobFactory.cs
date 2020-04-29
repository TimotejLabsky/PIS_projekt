using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Pis.Projekt.Business.Scheduling
{
    public class WeeklyJobFactory : IJobFactory
    {
        public WeeklyJobFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _scopeRegistry = new Dictionary<IJob, IServiceScope>();
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var job =  serviceProvider.GetRequiredService<WeeklyOptimizationJob>();
            _scopeRegistry[job] = scope;
            return job;
        }

        public void ReturnJob(IJob job)
        {
            _scopeRegistry.Remove(job);
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IDictionary<IJob, IServiceScope> _scopeRegistry;
    }
}