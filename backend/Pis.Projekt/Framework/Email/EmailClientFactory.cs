using System;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pis.Projekt.Framework.Email.Impl;

namespace Pis.Projekt.Framework.Email
{
    public class EmailClientFactory
    {
        public EmailClientFactory(IOptions<EmailClientFactoryConfiguration> configuration, SmtpClient smtpClient, ILogger<SmtpClientAdapter> smtpLogger,
            ILogger<WsdlEmailClient> wsdlLogger)
        {
            _configuration = configuration.Value;
            _smtpClient = smtpClient;
            _smtpLogger = smtpLogger;
            _wsdlLogger = wsdlLogger;
        }

        public IEmailClient Get()
        {
            switch (_configuration.ClientType)
            {
                case EmailClientFactoryConfiguration.EmailClientType.Wsdl:
                    return new WsdlEmailClient(_configuration.WsdlEmailClientConfiguration,
                        _configuration.WsdlConfiguration, _wsdlLogger);
                case EmailClientFactoryConfiguration.EmailClientType.Smtp:
                    return new SmtpClientAdapter(_configuration.SmtpClientConfiguration, _smtpClient, _smtpLogger);
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unknown type of Email Client: {_configuration.ClientType}");
            }
        }

        private readonly ILogger<WsdlEmailClient> _wsdlLogger;
        private readonly ILogger<SmtpClientAdapter> _smtpLogger;
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