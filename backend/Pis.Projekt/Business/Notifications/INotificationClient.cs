using System.Threading.Tasks;
using Pis.Projekt.Business.Notifications.Domain;

namespace Pis.Projekt.Business.Notifications
{
    public interface INotificationClient<in TNotification, in TIContent> where TNotification
        : INotification<TIContent>
    {
        Task NotifyAsync<TContent>(TNotification request) where TContent : TIContent;
    }
}