using System;
using System.Threading.Tasks;
using FiitEmailService;
using Pis.Projekt.Business.Notifications;

namespace Pis.Projekt.Framework.Email.Impl
{
    public class WsdlEmailClient : IEmailClient
    {
        public WsdlEmailClient(WsdlEmailClientConfiguration configuration,
            WsdlConfiguration<WsdlEmailClient> wsdlConfiguration)
        {
            _configuration = configuration;
            _wsdlConfiguration = wsdlConfiguration;
            _client = new EmailPortTypeClient();
        }

        public Task NotifyAsync<TContent>(INotification<TContent> request)
        {
            await 
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