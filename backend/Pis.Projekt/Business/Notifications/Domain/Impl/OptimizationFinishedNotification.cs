using System;
using System.Net.Mail;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class OptimizationFinishedNotification : IEmailNotification
    {
        public OptimizationFinishedNotification(DateTime nextOn,
            NotificationConfiguration configuration)
        {
            _configuration = configuration;
            FinishedOn = DateTime.Now;
            NextOn = nextOn;
            NotificationType = "optimalization.finished";
        }

        public string NotificationType { get; set; }
        public IEmail Content { get; set; }
        public DateTime FinishedOn { get; }
        public DateTime NextOn { get; }

        public MailAddress ToMailAddress =>
            new MailAddress(_configuration.OptimizationFinishedToAddress);

        public string Subject => NotificationType;
        public string Message => $"Price Optimalization Finished on {FinishedOn}. \n" +
                                 $"Next Optimalization is planned on {NextOn}";

        private readonly NotificationConfiguration _configuration;
    }
}