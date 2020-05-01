using System.Threading.Tasks;

namespace Pis.Projekt.Business.Notifications
{
    public interface IOptimizationNotificationService
    {
        public Task NotifyEvaluationFinishedAsync();

        public Task SendEvaluationBegunNotificationAsync();
    }
}