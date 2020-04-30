using System;
using System.Net.Mail;

namespace Pis.Projekt.Business.Notifications
{
    
    
    public interface INotification<TContent>
    {
        public Type Type { get; set; }

        public TContent Content { get; set; }
    }
}