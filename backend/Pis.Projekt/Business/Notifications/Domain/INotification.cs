using System;

namespace Pis.Projekt.Business.Notifications.Domain
{
    public interface INotification<out TContent>
    {
        public string NotificationType { get; set; }

        public TContent Content { get; }
    }
}