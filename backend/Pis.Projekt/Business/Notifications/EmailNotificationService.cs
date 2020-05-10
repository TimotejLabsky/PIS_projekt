using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Notifications.Domain;
using Pis.Projekt.Business.Notifications.Domain.Impl;
using Pis.Projekt.Business.Validation;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications
{
    public class EmailNotificationService : IOptimizationNotificationService
    {
        public EmailNotificationService(INotificationClient<IEmailNotification, IEmail> client,
            ILogger<EmailValidationService> logger,
            IOptions<NotificationConfiguration<UserTaskRequiredNotification>>
                userTaskNotificationConfiguration,
            IOptions<NotificationConfiguration<OptimizationFinishedNotification>>
                optimizationFinishedNotificationConfiguration,
            IOptions<NotificationConfiguration<OptimizationBegunNotification>>
                optimizationBegunNotificationConfiguration)

        {
            _client = client;
            _logger = logger;
            _userTaskNotificationConfiguration = userTaskNotificationConfiguration;
            _optimizationFinishedNotificationConfiguration =
                optimizationFinishedNotificationConfiguration;
            _optimizationBegunNotificationConfiguration =
                optimizationBegunNotificationConfiguration;
        }

        public async Task NotifyAsync<TContent>(IEmailNotification notification)
            where TContent : IEmail
        {
            await _client.NotifyAsync<TContent>(notification);
        }

        public async Task NotifyOptimizationFinishedAsync(
            IEnumerable<PricedProduct> modifiedProducts)
        {
            _logger.LogDebug("Notifying about the finished of the optimization");
            await NotifyAsync<OptimizationFinishedNotification>(
                new OptimizationFinishedNotification(modifiedProducts,
                    _optimizationFinishedNotificationConfiguration));
        }

        public async Task NotifyOptimizationBegunAsync()
        {
            _logger.LogDebug("Notifying about the beginning of the optimization");
            await NotifyAsync<OptimizationBegunNotification>(
                new OptimizationBegunNotification(_optimizationBegunNotificationConfiguration));
        }

        public async Task NotifyUserTaskCreatedAsync()
        {
            await NotifyAsync<OptimizationBegunNotification>(
                new UserTaskRequiredNotification(_userTaskNotificationConfiguration));
        }

        private readonly IOptions<NotificationConfiguration<OptimizationFinishedNotification>>
            _optimizationFinishedNotificationConfiguration;

        private readonly IOptions<NotificationConfiguration<OptimizationBegunNotification>>
            _optimizationBegunNotificationConfiguration;

        private readonly IOptions<NotificationConfiguration<UserTaskRequiredNotification>>
            _userTaskNotificationConfiguration;

        private readonly INotificationClient<IEmailNotification, IEmail> _client;
        private readonly ILogger<EmailValidationService> _logger;
    }
}