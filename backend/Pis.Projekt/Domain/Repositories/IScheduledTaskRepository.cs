using System;
using System.Threading;
using System.Threading.Tasks;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories
{
    public interface IScheduledTaskRepository : IRepository<int, ScheduledTask>
    {
        Task SetResolvedAsync(int id, CancellationToken token = default);

        Task SetFailedAsync(int taskId, CancellationToken token = default);
    }
}