using System;
using System.Net.Mail;
using Newtonsoft.Json;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class UserTaskRequiredNotification : IEmailNotification
    {
        public UserTaskRequiredNotification(IEmail content, string to)
        {
            Content = content;
            To = to;
        }

        public string NotificationType { get; set; }
        public string To { get; }
        public UserTask UserTask { get; set; }
        public MailAddress ToMailAddress => new MailAddress(To);
        public string Subject => $"{typeof(Type)}: User Task Required";
        public string Message => JsonConvert.SerializeObject(UserTask);
        public IEmail Content { get; }
    }
}