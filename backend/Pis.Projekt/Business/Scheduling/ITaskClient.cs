using System.Threading.Tasks;

namespace Pis.Projekt.Business.Scheduling
{
    public interface ITaskClient
    {    
        Task<int> SendAsync(ScheduledTask scheduledTask);
        Task SetCompleteAsync(int taskValue);
    }
} 