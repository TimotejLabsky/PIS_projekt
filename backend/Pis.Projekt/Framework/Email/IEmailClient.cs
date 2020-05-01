using System.Threading.Tasks;

namespace Pis.Projekt.Framework.Email
{
    public interface IEmailClient
    {
        Task SendMailAsync(string subject, string message, string email);
    }
}