using System.Threading.Tasks;

namespace Pis.Projekt.Business.Scheduling
{
    public interface ISchedulableTask<TResult>
    {
        public ScheduledTask Schedule(int id);
    }
}