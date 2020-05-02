using System.Threading.Tasks;
using Quartz;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class OptimizationJob: IJob
    {
        public OptimizationJob(SalesOptimalizationService optimalizationService)
        {
            _optimalizationService = optimalizationService;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            // Evaluate deadline
            await _optimalizationService.OptimizeSalesAsync().ConfigureAwait(false);
        }

        private readonly SalesOptimalizationService _optimalizationService;
    }
}