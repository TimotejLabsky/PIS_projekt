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
            ILogger<EmailValidationService> logger,
            IOptions<NotificationConfiguration<UserTaskRequiredNotification>>
                userTaskNotificationConfiguration,
            IOptions<NotificationConfiguration<OptimizationFinishedNotification>>
                optimizationFinishedNotificationConfiguration,
            IOptions<NotificationConfiguration<OptimizationBegunNotification>> optimizationBegunNotificationConfiguration)

        {
            _client = client;
            _logger = logger;
            _userTaskNotificationConfiguration = userTaskNotificationConfiguration;
            _OptimizationFinishedNotificationConfiguration =
                optimizationFinishedNotificationConfiguration;
            _OptimizationBegunNotificationConfiguration = optimizationBegunNotificationConfiguration;
        }

        public async Task NotifyAsync<TContent>(IEmailNotification notification)
            where TContent : IEmail
        {
            await _client.NotifyAsync<TContent>(notification);
        }

        public async Task NotifyOptimizationFinishedAsync(DateTime nextOptimalizationOn)
        {
            _logger.LogDebug("Notifying about the finished of the optimization");
            await NotifyAsync<OptimizationFinishedNotification>(
                new OptimizationFinishedNotification(nextOptimalizationOn, _OptimizationFinishedNotificationConfiguration));
        }

        public async Task NotifyOptimizationBegunAsync()
        {
            _logger.LogDebug("Notifying about the beginning of the optimization");
            await NotifyAsync<OptimizationBegunNotification>(
                new OptimizationBegunNotification(_OptimizationBegunNotificationConfiguration));
        }

        public async Task NotifyUserTaskCreatedAsync()
        {
            await NotifyAsync<OptimizationBegunNotification>(
                new UserTaskRequiredNotification(_userTaskNotificationConfiguration));
        }

        private readonly IOptions<NotificationConfiguration<OptimizationFinishedNotification>>
            _OptimizationFinishedNotificationConfiguration;
        private readonly IOptions<NotificationConfiguration<OptimizationBegunNotification>>
            _OptimizationBegunNotificationConfiguration;

        private readonly IOptions<NotificationConfiguration<UserTaskRequiredNotification>>
            _userTaskNotificationConfiguration;

        private readonly INotificationClient<IEmailNotification, IEmail> _client;
        private readonly ILogger<EmailValidationService> _logger;
    }
}