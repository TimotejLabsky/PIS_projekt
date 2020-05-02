using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Business.Notifications;
using Pis.Projekt.Business.Notifications.Domain;

namespace Pis.Projekt.Framework.Email.Impl
{
    public class SmtpClientAdapter : IEmailClient,
        INotificationClient<IEmailNotification, IEmail>
    {

        public SmtpClientAdapter(IOptions<SmtpClientConfiguration> configuration,
            SmtpClient client,
            ILogger<SmtpClientAdapter> logger)
        {
            _configuration = configuration.Value;
            _client = client;
            _logger = logger;
        }

        public async Task NotifyAsync<TContent>(IEmailNotification request) where TContent : IEmail
        {
            var email = request.Content;
            _logger.LogInformation($"Sending Notification via SMTP to: {email.ToMailAddress}, " +
                                   $"Subject: {email.Subject}, " +
                                   $"Message: {email.Message}");
            await SendMailAsync(email.Subject, email.Message, email.ToMailAddress.Address)
                .ConfigureAwait(false);
            
        }

        public async Task SendMailAsync(string subject, string message, string email)
        {
            await _client
                .SendMailAsync(_configuration.From, email, subject, message)
                .ConfigureAwait(false);
            _logger.LogDebug($"Email sent via SMTP to: {email}, " +
                                   $"Subject: {subject}, " +
                                   $"Message: {message}");
        }

        private readonly SmtpClient _client;
        private readonly SmtpClientConfiguration _configuration;
        private readonly ILogger<SmtpClientAdapter> _logger;
    }
}