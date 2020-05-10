using System;
using System.Threading.Tasks;
using FiitEmailService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Notifications.Domain;
using Pis.Projekt.System;

namespace Pis.Projekt.Framework.Email.Impl
{
    public class WsdlEmailClient : IEmailClient,
        INotificationClient<IEmailNotification, IEmail>
    {
        public WsdlEmailClient(IOptions<WsdlEmailClientConfiguration> configuration,
            IOptions<WsdlConfiguration<WsdlEmailClient>> wsdlConfiguration,
            ILogger<WsdlEmailClient> logger)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _wsdlConfiguration = wsdlConfiguration.Value;
            _client = new EmailPortTypeClient();
        }

        public async Task NotifyAsync<TContent>(IEmailNotification request)
            where TContent : IEmail
        {
            await SendMailAsync(request.Subject, request.Message, request.ToMailAddress?.Address)
                .ConfigureAwait(false);
        }

        public async Task SendMailAsync(string subject, string message, string email = null)
        {
            email ??= _configuration.DefaultToAddress;

            _logger.LogDevelopment(
                $"Email sent. Subject: {subject}, To: {email}, Message: {message}");
            var response = await _client.notifyAsync(TeamId, Password, email, subject, message)
                .ConfigureAwait(false);

            if (!response.success)
            {
                throw new InvalidOperationException(
                    $"Email {subject} could not be sent to {email}");
            }
        }

        private string TeamId => _wsdlConfiguration.TeamId;
        private string Password => _wsdlConfiguration.Password;

        // ReSharper disable once NotAccessedField.Local - Used for production
        private readonly EmailPortTypeClient _client;
        private readonly WsdlEmailClientConfiguration _configuration;
        private readonly WsdlConfiguration<WsdlEmailClient> _wsdlConfiguration;
        private readonly ILogger<WsdlEmailClient> _logger;
    }
}