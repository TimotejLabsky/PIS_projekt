using System;
using System.Net.Mail;
using Pis.Projekt.Framework.Email.Impl;

namespace Pis.Projekt.Framework.Email
{
    public class EmailClientFactory
    {
        public EmailClientFactory(EmailClientFactoryConfiguration configuration, SmtpClient smtpClient)
        {
            _configuration = configuration;
            _smtpClient = smtpClient;
        }

        public IEmailClient Get()
        {
            switch (_configuration.ClientType)
            {
                case EmailClientFactoryConfiguration.EmailClientType.Wsdl:
                    return new WsdlEmailClient(_configuration.WsdlEmailClientConfiguration,
                        _configuration.WsdlConfiguration);
                case EmailClientFactoryConfiguration.EmailClientType.Smtp:
                    return new SmtpClientAdapter(_configuration.SmtpClientConfiguration, _smtpClient);
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unknown type of Email Client: {_configuration.ClientType}");
            }
        }

        private readonly EmailClientFactoryConfiguration _configuration;
        private readonly SmtpClient _smtpClient;
    }

    public class EmailClientFactoryConfiguration
    {
        public EmailClientType ClientType { get; set; }
        public SmtpClientConfiguration SmtpClientConfiguration { get; set; }
        public WsdlEmailClientConfiguration WsdlEmailClientConfiguration { get; set; }
        public WsdlConfiguration<WsdlEmailClient> WsdlConfiguration { get; set; }

        public enum EmailClientType
        {
            Wsdl,
            Smtp
        }
    }
}