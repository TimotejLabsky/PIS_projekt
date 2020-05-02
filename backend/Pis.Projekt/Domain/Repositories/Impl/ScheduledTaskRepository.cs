using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Business.Scheduling;
using Pis.Projekt.Domain.Database.Contexts;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Repositories.Impl
{
    // TODO probably will not be used, TaskWsdl Service provides this functionality
    public class ScheduledTaskRepository :
        AbstractEFRepository<SalesDbContext, int, ScheduledTask>, IScheduledTaskRepository,
        IDisposable
    {
        public ScheduledTaskRepository(IServiceScopeFactory scopeFactory,
            ILogger<ScheduledTaskRepository> logger) : base(scopeFactory)
        {
            _logger = logger;
            // empty
        }

        protected override DbSet<ScheduledTask> Entities => DbContext.ScheduledTasks;

        public async Task SetResolvedAsync(int id, CancellationToken token = default)
        {
            var taskEntity = await RequireAsync(id, token).ConfigureAwait(false);

            if (taskEntity.IsResolved)
            {
                throw new InvalidOperationException(
                    $"Task {id}:{taskEntity.Name} has been already tagged resolved");
            }

            taskEntity.IsResolved = true;

            await UpdateAsync(taskEntity, token).ConfigureAwait(false);
            _logger.LogInformation($"Task {id}:{taskEntity.Name} has been resolved", taskEntity);
        }

        public async Task SetFailedAsync(int taskId, CancellationToken token = default)
        {
            var taskEntity = await RequireAsync(taskId, token).ConfigureAwait(false);

            if (taskEntity.IsResolved)
            {
                throw new InvalidOperationException(
                    $"Task {taskId}:{taskEntity.Name} has been already resolved");
            }

            if (taskEntity.IsResolved)
            {
                throw new InvalidOperationException(
                    $"Task {taskId}:{taskEntity.Name} has failed already");
            }

            taskEntity.IsResolved = true;

            await UpdateAsync(taskEntity, token).ConfigureAwait(false);
            _logger.LogInformation($"Task {taskId}:{taskEntity.Name} has been resolved",
                taskEntity);
        }

        private readonly ILogger<ScheduledTaskRepository> _logger;
    }
}