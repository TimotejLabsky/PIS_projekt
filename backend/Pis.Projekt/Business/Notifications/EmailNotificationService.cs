using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pis.Projekt.Business.Notifications.Domain;
using Pis.Projekt.Business.Notifications.Domain.Impl;
using Pis.Projekt.Business.Scheduling;
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
                optimizationBegunNotificationConfiguration,
            IOptions<NotificationConfiguration<SeasonProductsPickedNotification>> seasonalConfig)

        {
            _client = client;
            _logger = logger;
            _userTaskNotificationConfiguration = userTaskNotificationConfiguration;
            _optimizationFinishedNotificationConfiguration =
                optimizationFinishedNotificationConfiguration;
            _optimizationBegunNotificationConfiguration =
                optimizationBegunNotificationConfiguration;
            _seasonalConfig = seasonalConfig;
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
            await NotifyAsync<UserTaskRequiredNotification>(
                new UserTaskRequiredNotification(_userTaskNotificationConfiguration));
        }

        public async Task NotifyUpdatedSeasonPrices(IEnumerable<TaskProduct> pickedProducts)
        {
            await NotifyAsync<UpdatedSeasonNotification>(
                new UpdatedSeasonNotification(pickedProducts, _seasonalConfig));
        }

        public async Task NotifyStoreChangedPrices(string store)
        {
            await NotifyAsync<SeasonalPriceChangedNotification>(
                new SeasonalPriceChangedNotification(store)).ConfigureAwait(false);
        }

        private readonly IOptions<NotificationConfiguration<OptimizationFinishedNotification>>
            _optimizationFinishedNotificationConfiguration;

        private readonly IOptions<NotificationConfiguration<OptimizationBegunNotification>>
            _optimizationBegunNotificationConfiguration;

        private readonly IOptions<NotificationConfiguration<UserTaskRequiredNotification>>
            _userTaskNotificationConfiguration;

        private readonly IOptions<NotificationConfiguration<SeasonProductsPickedNotification>>
            _seasonalConfig;

        private readonly INotificationClient<IEmailNotification, IEmail> _client;
        private readonly ILogger<EmailValidationService> _logger;
    }


    public class SeasonProductsPickedNotification : IEmailNotification
    {
        public SeasonProductsPickedNotification(IEnumerable<TaskProduct> pickedProducts,
            IOptions<NotificationConfiguration<SeasonProductsPickedNotification>> seasonalConfig)
        {
            _pickedSeasonalProducts = pickedProducts;
            _configuration = seasonalConfig.Value;
            NotificationType = UserTaskType.IncludeToSeason;
        }

        public string NotificationType { get; set; }
        public IEmail Content => this;
        public MailAddress ToMailAddress => new MailAddress(_configuration.ToAddress);
        public string Subject => NotificationType;

        public virtual string Message => $"Products where selected to be in next season\n" +
                                 $"Products: \n{ProductsJson}";

        public string ProductsJson =>
            JsonConvert.SerializeObject(_pickedSeasonalProducts, Formatting.Indented);

        private readonly IEnumerable<TaskProduct> _pickedSeasonalProducts;
        private readonly NotificationConfiguration<SeasonProductsPickedNotification> _configuration;
    }

    public class UpdatedSeasonNotification : SeasonProductsPickedNotification
    {
        public UpdatedSeasonNotification(IEnumerable<TaskProduct> pickedProducts,
            IOptions<NotificationConfiguration<SeasonProductsPickedNotification>> seasonalConfig) :
            base(pickedProducts, seasonalConfig)
        {
            NotificationType = "notify-seasonal-prices-updated";
        }

        public override string Message => "Prices of seasonal Products were modified\n" +
        $"Products: \n{ProductsJson}";
    }
    
    public class SeasonalPriceChangedNotification : IEmailNotification
    {
        public SeasonalPriceChangedNotification(string storeEmail)
        {
            NotificationType = "seasonal-prices-changed";
            ToMailAddress = new MailAddress(storeEmail);
        }

        public string NotificationType { get; set; }
        public IEmail Content => this;
        public MailAddress ToMailAddress { get; }
        public string Subject => NotificationType;

        public virtual string Message => $"Seasonal Products have new prices";
    }
}