using System.Threading.Tasks;

namespace Pis.Projekt.Business.Notifications
{
    public interface INotificationClient
    {
        Task NotifyAsync<TContent>(INotification<TContent> request);
    }
}