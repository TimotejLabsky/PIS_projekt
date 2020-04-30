using System.Threading.Tasks;
using FiitEmailService;
using Pis.Projekt.Business.Notifications;

namespace Pis.Projekt.Framework.Email
{
    public interface IEmailClient
    {
        Task NotifyAsync<TContent>(INotification<TContent> request);

        Task SendMailAsync(string subject, string message, string email = null);
    }
}