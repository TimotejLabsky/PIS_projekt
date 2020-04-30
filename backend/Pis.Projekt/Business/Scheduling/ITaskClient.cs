using System.Threading.Tasks;

namespace Pis.Projekt.Business.Scheduling
{
    public interface ITaskClient
    {
        Task SendAsync(ScheduledTask scheduledTask);
    }
}