using System;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Business.Scheduling.Impl;
using Quartz;
using Quartz.Spi;

namespace Pis.Projekt.Business.Scheduling
{
    // Done
    public class OptimizationJobFactory : IJobFactory
    {
        public OptimizationJobFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _provider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        public IJob CreateOptimizationJob()
        {
            return _provider.GetRequiredService<OptimizationJob>();
        }


        public IJob CreateProductSalesDecreasedTimeoutJob(ProductSalesDecreasedTask task)
        {
            var job = _provider.GetRequiredService<UserEvaluationJob>();
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