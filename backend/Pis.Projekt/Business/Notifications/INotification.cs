using System;

namespace Pis.Projekt.Business.Notifications
{
    public interface INotification<TContent>
    {
        public Type Type { get; set; }

        public TContent Content { get; set; }

        //TODO add To and From fields
    }
}