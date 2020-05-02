using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Notifications.Domain;

namespace Pis.Projekt.Framework.Email.Impl
{
    public class SmtpClientAdapter : IEmailClient,
        INotificationClient<IEmailNotification, IEmail>
    {

        public SmtpClientAdapter(IOptions<SmtpClientConfiguration> configuration,            SmtpClient client)
        {
            _configuration = configuration.Value;
            _client = client;
        }

        public async Task NotifyAsync<TContent>(IEmailNotification request) where TContent : IEmail
        {
            var email = request.Content;
            await SendMailAsync(email.Subject, email.Message, email.ToMailAddress.Address)
                .ConfigureAwait(false);
        }

        public async Task SendMailAsync(string subject, string message, string email)
        {
            await _client
                .SendMailAsync(_configuration.From, email, subject, message)
                .ConfigureAwait(false);
        }

        private readonly SmtpClient _client;
        private readonly SmtpClientConfiguration _configuration;
    }
}