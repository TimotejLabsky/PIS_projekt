using System.Threading.Tasks;
using Pis.Projekt.Business.Notifications.Domain.Impl;

namespace Pis.Projekt.Business.Notifications
{
    public class NotificationService: IOptimizationNotificationService
    {
        public NotificationService(INotificationClient client, NotificationConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
        
        public async Task Notify<TContent>(INotification<TContent> notification)
        {
            await _client.NotifyAsync(notification);
        }
        
        public async Task NotifyEvaluationFinishedAsync()
        {
            await Notify(new OptimizationFinishedNotification(_configuration));
        }
        
        public async Task SendEvaluationBegunNotificationAsync()
        {
            await Notify(new OptimizationFinishedNotification(_configuration));
        }
        

        private readonly NotificationConfiguration _configuration;
        private readonly INotificationClient _client;
    }
}