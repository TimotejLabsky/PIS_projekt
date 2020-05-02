using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Notifications.Domain;
using Pis.Projekt.Business.Notifications.Domain.Impl;

namespace Pis.Projekt.Business.Notifications
{
    public class EmailNotificationService : IOptimizationNotificationService
    {
        public EmailNotificationService(INotificationClient<IEmailNotification, IEmail> client,
            IOptions<NotificationConfiguration> configuration)
        {
            _client = client;
            _configuration = configuration.Value;
        }

        public async Task NotifyAsync<TContent>(IEmailNotification notification)
            where TContent : IEmail
        {
            await _client.NotifyAsync<TContent>(notification);
        }

        public async Task NotifyOptimizationFinishedAsync(DateTime nextOptimalizationOn)
        {
            await NotifyAsync<OptimizationFinishedNotification>(new OptimizationFinishedNotification(nextOptimalizationOn, _configuration));
        }

        public async Task NotifyOptimizationBegunAsync()
        {
            await NotifyAsync<OptimizationBegunNotification>(new OptimizationBegunNotification(_configuration));
        }

        private readonly NotificationConfiguration _configuration;
        private readonly INotificationClient<IEmailNotification, IEmail> _client;
    }
}