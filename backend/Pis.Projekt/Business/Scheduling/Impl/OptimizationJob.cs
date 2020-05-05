using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class OptimizationJob : IJob
    {
        public OptimizationJob(ILogger<OptimizationJob> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            // Evaluate deadline
            _logger.LogError($"Executing optimization job at {DateTime.Now}");

            var optimalizationService = scope.ServiceProvider
                .GetRequiredService<SalesOptimalizationService>();
            //await optimalizationService.OptimizeSalesAsync().ConfigureAwait(false);
        }

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OptimizationJob> _logger;
    }
}