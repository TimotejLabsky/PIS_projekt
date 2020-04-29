using System.Threading.Tasks;

namespace Pis.Projekt.Business.Notifications
{
    public class NotificationService
    {
        public Task SendNotification<TContent>(INotification<TContent> notification)
        {
            // Serialize and send
            return Task.CompletedTask;
        }
        
        public Task SendEvaluationFinishedNotificationAsync()
        {
            return Task.CompletedTask;
        }
        
        public Task SendEvaluationBegunNotificationAsync()
        {
            return Task.CompletedTask;
        }

        public Task NotifyAsync(OptimalizationFinishedStoreNotification notification)
        {
            return Task.CompletedTask;
        }
        
        public Task NotifyAsync(UserTaskRequiredNotification notification)
        {
            return Task.CompletedTask;
        }
    }
}