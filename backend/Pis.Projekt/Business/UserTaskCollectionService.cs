using System;
using System.Collections.Generic;
using System.Linq;
using Pis.Projekt.Business.Scheduling;

namespace Pis.Projekt.Business
{
    public class UserTaskCollectionService
    {
        public UserTaskCollectionService()
        {
            _scheduledTasks = new List<ScheduledTask>();
        }

        public void Register(ScheduledTask task)
        {
            _scheduledTasks.Add(task);
        }

        public ScheduledTask Find(Guid id)
        {
            return _scheduledTasks.Where(s => s.Id == id).First();
        }

        private readonly IList<ScheduledTask> _scheduledTasks;
    }
}