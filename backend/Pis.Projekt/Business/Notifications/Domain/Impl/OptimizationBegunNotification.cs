using System;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class OptimizationBegunNotification : IEmailNotification
    {
        public OptimizationBegunNotification(IOptions<NotificationConfiguration> configuration)
        {
            _configuration = configuration.Value;
            BegunOn = DateTime.Now;
        }

        public string NotificationType { get; set; }
        public IEmail Content => this;
        public DateTime BegunOn { get; }

        public MailAddress ToMailAddress =>
            new MailAddress(_configuration.OptimizationFinishedToAddress);

        public string Subject => $"{NotificationType}";
        public string Message => $"Price Optimalization Begun on {BegunOn}";

        private readonly NotificationConfiguration _configuration;
    }
}