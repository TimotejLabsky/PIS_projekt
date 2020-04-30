using System;
using System.Net.Mail;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications
{
    public class UserTaskRequiredNotification : IEmailNotification<UserTask>
    {
        public Type Type { get; set; }
        public UserTask Content { get; set; }

        public static UserTaskRequiredNotification Create(UserTask userTask)
        {
            return new UserTaskRequiredNotification
            {
                Type = typeof(UserTaskRequiredNotification),
                Content = userTask
            };
        }

        public MailAddress ToMailAddress { get; }
        public string Subject { get; }
        public string Message { get; }
    }
}