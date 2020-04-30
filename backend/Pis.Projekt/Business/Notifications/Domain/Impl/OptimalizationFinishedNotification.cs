using System;
using System.Net.Mail;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class OptimalizationFinishedNotification: IEmailNotification<OptimalizationFinishedState>
    {
        public OptimalizationFinishedNotification(NotificationConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Type Type { get; set; }
        public OptimalizationFinishedState Content { get; set; }
        public MailAddress ToMailAddress => 
        public string Subject { get; }
        public string Message { get; }

        private readonly NotificationConfiguration _configuration;
    }

    public class OptimalizationFinishedState
    {
        
    }
}