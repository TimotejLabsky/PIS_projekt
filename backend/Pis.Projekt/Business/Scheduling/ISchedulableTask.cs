using System.Threading.Tasks;
using Pis.Projekt.Business.Scheduling.Impl;

namespace Pis.Projekt.Business.Scheduling
{
    public interface ISchedulableTask<TResult> : ITask<TResult>
    {
        public ScheduledTask Schedule(int id);
        
        event TaskFulfilledHandler OnTaskFulfilled;
        event TaskFailedHandler OnTaskFailed;

        public delegate Task TaskFulfilledHandler(ScheduledTaskResult result);
        public delegate Task TaskFailedHandler(ScheduledTask failedTask);
    }
}