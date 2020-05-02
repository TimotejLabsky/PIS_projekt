using System.Threading.Tasks;

namespace Pis.Projekt.Business.Scheduling
{
    public interface ISchedulabletask<TResult> : ITask<TResult>
    {
        public ScheduledTask Schedule(int id);
        
        event TaskFulfilledHandler OnTaskFulfilled;
        event TaskFailedHandler OnTaskFailed;

        public void Evaluate();

        public void Fulfill();

        public delegate Task TaskFulfilledHandler(ScheduledTaskResult result);
        public delegate Task TaskFailedHandler(ScheduledTask failedTask);
    }
}