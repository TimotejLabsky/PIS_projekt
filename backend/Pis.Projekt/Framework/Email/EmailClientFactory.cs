using System;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Pis.Projekt.Framework.Email.Impl;

namespace Pis.Projekt.Framework.Email
{
    public class EmailClientFactory
    {
        public EmailClientFactory(IOptions<EmailClientFactoryConfiguration> configuration, SmtpClient smtpClient)
        {
            _configuration = configuration.Value;
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
        public IOptions<SmtpClientConfiguration> SmtpClientConfiguration { get; set; }
        public IOptions<WsdlEmailClientConfiguration> WsdlEmailClientConfiguration { get; set; }
        public IOptions<WsdlConfiguration<WsdlEmailClient>> WsdlConfiguration { get; set; }

        public enum EmailClientType
        {
            Wsdl,
            Smtp
        }
    }
}