namespace Pis.Projekt.Business.Notifications.Domain
{
    public interface IEmailNotification : INotification<IEmail>, IEmail
    {
        // empty
    }
}