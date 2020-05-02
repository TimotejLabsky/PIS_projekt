using System;
using System.Threading.Tasks;
using FiitEmailService;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Notifications.Domain;

namespace Pis.Projekt.Framework.Email.Impl
{
    public class WsdlEmailClient : IEmailClient,
        INotificationClient<IEmailNotification, IEmail>
    {
        public WsdlEmailClient(IOptions<WsdlEmailClientConfiguration> configuration,
            IOptions<WsdlConfiguration<WsdlEmailClient>> wsdlConfiguration)
        {
            _configuration = configuration.Value;
            _wsdlConfiguration = wsdlConfiguration.Value;
            _client = new EmailPortTypeClient();
        }

        public async Task NotifyAsync<TContent>(IEmailNotification request)
            where TContent : IEmail
        {
            var email = request.Content;
            await SendMailAsync(email.Subject, email.Message, email.ToMailAddress.Address)
                .ConfigureAwait(false);
        }

        public async Task SendMailAsync(string subject, string message, string email = null)
        {
            email ??= _configuration.DefaultToAddress;
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

        private readonly EmailPortTypeClient _client;
        private readonly WsdlEmailClientConfiguration _configuration;
        private readonly WsdlConfiguration<WsdlEmailClient> _wsdlConfiguration;
    }
}