using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Business.Scheduling.Impl;

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

        // public static CronExpression ReadCron(this IConfiguration configuration, string section)
        // {
        //     return configuration.

        // }
    }
}