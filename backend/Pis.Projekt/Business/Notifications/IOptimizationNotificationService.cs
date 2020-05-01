using System;
using System.Threading.Tasks;

namespace Pis.Projekt.Business.Notifications
{
    public interface IOptimizationNotificationService
    {
        public Task NotifyOptimizationFinishedAsync(DateTime nextOptimalizationOn);

        public Task NotifyOptimizationBegunAsync();
    }
}