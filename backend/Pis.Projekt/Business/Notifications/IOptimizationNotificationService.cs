using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications
{
    public interface IOptimizationNotificationService
    {
        Task NotifyOptimizationFinishedAsync(IEnumerable<PricedProduct> nextOptimalizationOn);
        Task NotifyOptimizationBegunAsync();
        Task NotifyUserTaskCreatedAsync();
        Task NotifyUpdatedSeasonPrices(IEnumerable<TaskProduct> pickedProducts);
    }
}