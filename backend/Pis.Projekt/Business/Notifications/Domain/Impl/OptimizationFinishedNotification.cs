using System;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class OptimizationFinishedNotification : IEmailNotification
    {
        public OptimizationFinishedNotification(DateTime nextOn,
            IOptions<NotificationConfiguration<OptimizationFinishedNotification>> configuration)
        {
            _configuration = configuration.Value;
            FinishedOn = DateTime.Now;
            NextOn = nextOn;
            NotificationType = "optimalization.finished";
        }

        public string NotificationType { get; set; }
        public IEmail Content { get; set; }
        public DateTime FinishedOn { get; }
        public DateTime NextOn { get; }

        public MailAddress ToMailAddress =>
            new MailAddress(_configuration.ToAddress);

        public string Subject => NotificationType;
        public string Message => $"Price Optimalization Finished on {FinishedOn}. \n" +
                                 $"Next Optimalization is planned on {NextOn}";

        private readonly NotificationConfiguration<OptimizationFinishedNotification> _configuration;
    }
}