using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Notifications.Domain.Impl;

namespace Pis.Projekt.Business.Validation
{
    public class ConfigEmailValidation
    {
        public ConfigEmailValidation(IConfiguration configuration, EmailValidationService validationService,
            ILogger<ConfigEmailValidation> logger)
        {
            _configuration = configuration;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task Validate()
        {
            await ValidateEmail(
                _configuration.GetValue<string>("NotificationService:OptimizationBegunNotification:ToAddress")
                , nameof(OptimizationBegunNotification)).ConfigureAwait(false);
            await ValidateEmail(_configuration.GetValue<string>("NotificationService:UserTaskNotification:ToAddress")
                , nameof(UserTaskRequiredNotification)).ConfigureAwait(false);
            await ValidateEmail(
                _configuration.GetValue<string>("NotificationService:OptimizationFinishedNotification:ToAddress")
                , nameof(OptimizationFinishedNotification)).ConfigureAwait(false);
        }

        private async Task ValidateEmail(string email, string notificationType)
        {
            try
            {
                await _validationService.ValidateEmail(email).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Nebolo možné zvalidovať email {email} pre upozornenie typu {notificationType}");
                throw;
            }
        }

        private readonly ILogger<ConfigEmailValidation> _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailValidationService _validationService;
    }
}