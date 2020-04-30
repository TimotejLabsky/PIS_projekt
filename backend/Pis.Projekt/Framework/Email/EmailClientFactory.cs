using Pis.Projekt.Framework.Email.Impl;

namespace Pis.Projekt.Framework.Email
{
    public class EmailClientFactory
    {
        public IEmailClient Get<TEmailClient>() where TEmailClient : IEmailClient
        {
            var clientType = typeof(TEmailClient);
            if (clientType == typeof(WsdlEmailClient))
            {
                return new WsdlEmailClient();
            }
            
            if (clientType == typeof(SmtpClient))
            {
                return new SmtpClient();
            }
        }

        private readonly SmtpClientConfiguration _smtpClientConfiguration;
    }
}