using System;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications
{
    public class UserTaskRequiredNotification : INotification<UserTask>
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
    }
}