using System;
using System.Net.Mail;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class OptimizationFinishedNotification: IEmailNotification<OptimizationFinishedState>
    {
        public OptimizationFinishedNotification(NotificationConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Type Type { get; set; }
        public OptimizationFinishedState Content { get; set; }
        public MailAddress ToMailAddress => 
        public string Subject { get; }
        public string Message { get; }

        private readonly NotificationConfiguration _configuration;
    }

    public class OptimizationFinishedState
    {
        
    }
}