using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class OptimizationJob: IJob
    {
        public OptimizationJob(SalesOptimalizationService optimalizationService, ILogger<OptimizationJob> logger)
        {
            _optimalizationService = optimalizationService;
            _logger = logger;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            // Evaluate deadline
            _logger.LogDebug("Executing optimization job");
            await _optimalizationService.OptimizeSalesAsync().ConfigureAwait(false);
        }

        private readonly SalesOptimalizationService _optimalizationService;
        private readonly ILogger<OptimizationJob> _logger;
    }
}