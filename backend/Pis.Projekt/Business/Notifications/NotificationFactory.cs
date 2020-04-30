using System;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace Pis.Projekt.Business.Notifications
{
    public class NotificationFactory
    {
        public NotificationFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public INotification<TContent> Get<TContent>()
        {
            return _provider.GetRequiredService<INotification<TContent>>();
        }

        private readonly IServiceProvider _provider;
    }
}