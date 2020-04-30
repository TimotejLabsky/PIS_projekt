using System.Threading.Tasks;
using FiitEmailService;
using Pis.Projekt.Business.Notifications;

namespace Pis.Projekt.Framework.Email.Impl
{
    public class WsdlEmailClient: IEmailClient
    {
        public WsdlEmailClient(WsdlEmailClientConfiguration configuration)
        {
            _configuration = configuration;
            _client = new EmailPortTypeClient();
        }

        public Task<notifyResponse> NotifyAsync(notifyRequest request)
        {
            return ((EmailPortType) _client).notifyAsync(request);
        }

        private readonly EmailPortTypeClient _client;
        private readonly WsdlEmailClientConfiguration _configuration;
        public Task<notifyResponse> NotifyAsync<TContent>(INotification<TContent> request)
        {
            throw new System.NotImplementedException();
        }
    }
}