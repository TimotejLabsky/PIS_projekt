using System;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class UserTaskRequiredNotification : IEmailNotification
    {
        public UserTaskRequiredNotification(
            IOptions<NotificationConfiguration<UserTaskRequiredNotification>> configuration)
        {
            _configuration = configuration.Value;
        }

        public string NotificationType { get; set; }
        public UserTask UserTask { get; set; }

        public MailAddress ToMailAddress =>
            new MailAddress(_configuration.ToAddress);

        public string Subject => $"{typeof(Type)}: User Task Required";
        public string Message => JsonConvert.SerializeObject(UserTask);
        public IEmail Content => this;
        private readonly NotificationConfiguration<UserTaskRequiredNotification> _configuration;
    }
}