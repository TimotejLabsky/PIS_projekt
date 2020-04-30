using System.Net.Mail;

namespace Pis.Projekt.Business.Notifications
{
    public interface IEmail
    {
        MailAddress ToMailAddress { get; }
        string Subject { get; }
        string Message { get; }
    }
}