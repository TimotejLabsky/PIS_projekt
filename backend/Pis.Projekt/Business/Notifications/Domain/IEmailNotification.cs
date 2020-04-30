namespace Pis.Projekt.Business.Notifications
{
    public interface IEmailNotification<TContent> : INotification<TContent>, IEmail
    {
        // empty
    }
}