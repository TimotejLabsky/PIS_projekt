using System.Threading.Tasks;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Notifications.Domain;

namespace Pis.Projekt.Framework.Email.Impl
{
    public class SmtpClientAdapter : IEmailClient,
        INotificationClient<IEmailNotification, IEmail>
    {
        public SmtpClientAdapter(SmtpClientConfiguration configuration,
            System.Net.Mail.SmtpClient client)
        {
            _configuration = configuration;
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

        private readonly System.Net.Mail.SmtpClient _client;
        private readonly SmtpClientConfiguration _configuration;
    }
}