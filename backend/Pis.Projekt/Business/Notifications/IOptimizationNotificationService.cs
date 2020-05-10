using System;
using System.Threading.Tasks;

namespace Pis.Projekt.Business.Notifications
{
    public interface IOptimizationNotificationService
    {
        Task NotifyOptimizationFinishedAsync(DateTime nextOptimalizationOn);
        Task NotifyOptimizationBegunAsync();
        Task NotifyUserTaskCreatedAsync();
    }
}