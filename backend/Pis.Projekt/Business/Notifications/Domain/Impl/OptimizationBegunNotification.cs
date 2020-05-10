using System;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class OptimizationBegunNotification : IEmailNotification
    {
        public OptimizationBegunNotification(IOptions<NotificationConfiguration<OptimizationBegunNotification>> configuration)
        {
            _configuration = configuration.Value;
            BegunOn = DateTime.Now;
            NotificationType = "optimization-begun";
        }

        public string NotificationType { get; set; }
        public IEmail Content => this;
        public DateTime BegunOn { get; }

        public MailAddress ToMailAddress =>
            new MailAddress(_configuration.ToAddress);

        public string Subject => $"{NotificationType} at {BegunOn}";
        public string Message => $"Price Optimalization Begun on {BegunOn}";

        private readonly NotificationConfiguration<OptimizationBegunNotification> _configuration;
    }
}