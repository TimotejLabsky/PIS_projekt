using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Business.Scheduling.Impl;
using Quartz;

namespace Pis.Projekt.System
{
    public static class StartupSchedulerExtensions
    {
        public static void ConfigureScheduler(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<OptimizationJob>();
            services.AddSingleton(new JobSchedule<OptimizationJob>("0/60 * * * * ?"));
        }

        public static CronExpression ReadCron(this IConfiguration configuration, string section)
        {
            return configuration.
        }
    }
}