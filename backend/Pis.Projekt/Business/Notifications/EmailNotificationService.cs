using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Notifications.Domain;
using Pis.Projekt.Business.Notifications.Domain.Impl;
using Pis.Projekt.Business.Validation;

namespace Pis.Projekt.Business.Notifications
{
    public class EmailNotificationService : IOptimizationNotificationService
    {
        public EmailNotificationService(INotificationClient<IEmailNotification, IEmail> client,
            IOptions<NotificationConfiguration> configuration, ILogger<EmailValidationService> logger)

        {
            _client = client;
            _configuration = configuration;
              _logger = logger;
        }

        public async Task NotifyAsync<TContent>(IEmailNotification notification)
            where TContent : IEmail
        {
            await _client.NotifyAsync<TContent>(notification);
        }

        public async Task NotifyOptimizationFinishedAsync(DateTime nextOptimalizationOn)
        {
            _logger.LogDebug("Notifying about the finished of the optimization");
            await NotifyAsync<OptimizationFinishedNotification>(new OptimizationFinishedNotification(nextOptimalizationOn, _configuration));
        }

        public async Task NotifyOptimizationBegunAsync()
        {
            _logger.LogDebug("Notifying about the beginning of the optimization");
            await NotifyAsync<OptimizationBegunNotification>(new OptimizationBegunNotification(_configuration));
        }

        private readonly IOptions<NotificationConfiguration> _configuration;
        private readonly INotificationClient<IEmailNotification, IEmail> _client;
        private readonly ILogger<EmailValidationService> _logger;
    }
}