using System.Threading.Tasks;
using Pis.Projekt.Framework.Email;

namespace Pis.Projekt.Business.Notifications
{
    public class NotificationService
    {
        public NotificationService(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }
        
        public Task SendNotification<TContent>(INotification<TContent> notification)
        {
            // Serialize and send
            var client = new FiitEmailService
                    .EmailPortTypeClient();

            client.notifyAsync("017", "FMLCCJ", "kikprofik@gmail.com", "Hello World", "Bigpp");
            return Task.CompletedTask;
        }
        
        public Task SendEvaluationFinishedNotificationAsync()
        {
            await _emailClient.notifyAsync("017", "FMLCCJ", "kikprofik@gmail.com", 
                "Hello World", "Bigpp").ConfigureAwait(false);
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

        private readonly IEmailClient _emailClient;
    }
}