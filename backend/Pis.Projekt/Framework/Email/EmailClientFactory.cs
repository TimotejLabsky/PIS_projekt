using System;
using Pis.Projekt.Framework.Email.Impl;

namespace Pis.Projekt.Framework.Email
{
    public class EmailClientFactory
    {
        public EmailClientFactory(EmailClientFactoryConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEmailClient Get()
        {
            switch (_configuration.ClientType)
            {
                case EmailClientFactoryConfiguration.EmailClientType.Wsdl:
                    return new WsdlEmailClient(_configuration.WsdlEmailClientConfiguration);
                case EmailClientFactoryConfiguration.EmailClientType.Smtp:
                    return new SmtpClient(_configuration.SmtpClientConfiguration);
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unknown type of Email Client: {_configuration.ClientType}");
            }
        }

        private readonly EmailClientFactoryConfiguration _configuration;
    }

    public class EmailClientFactoryConfiguration
    {
        public EmailClientType ClientType { get; set; }
        public SmtpClientConfiguration SmtpClientConfiguration { get; set; }
        public WsdlEmailClientConfiguration WsdlEmailClientConfiguration { get; set; }

        public enum EmailClientType
        {
            Wsdl,
            Smtp
        }
    }
}