using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Notifications.Domain.Impl
{
    public class OptimizationFinishedNotification : IEmailNotification
    {
        public OptimizationFinishedNotification(IEnumerable<PricedProduct> modified,
            IOptions<NotificationConfiguration<OptimizationFinishedNotification>> configuration)
        {
            _configuration = configuration.Value;
            FinishedOn = DateTime.Now;
            NotificationType = "optimalization.finished";
            Modified = modified;
        }

        public string NotificationType { get; set; }
        public IEmail Content => this;
        public DateTime FinishedOn { get; }
        public IEnumerable<PricedProduct> Modified { get; }

        public MailAddress ToMailAddress =>
            new MailAddress(_configuration.ToAddress);

        public string Subject => NotificationType;
        public string Message => $"Price Optimalization Finished on {FinishedOn}. \n" +
                                 $"New Prices: {ModifiedAsJson}";

        private string ModifiedAsJson => JsonConvert.SerializeObject(Modified, Formatting.Indented);

        private readonly NotificationConfiguration<OptimizationFinishedNotification> _configuration;
    }
}