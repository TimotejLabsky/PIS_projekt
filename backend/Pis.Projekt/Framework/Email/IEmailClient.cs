using System.Threading.Tasks;
using FiitEmailService;
using Pis.Projekt.Business.Notifications;

namespace Pis.Projekt.Framework.Email
{
    public interface IEmailClient
    {
        Task SendMailAsync(string subject, string message, string email = null);
    }
}