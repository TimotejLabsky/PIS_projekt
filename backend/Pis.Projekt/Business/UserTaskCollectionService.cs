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
            _scheduledTasks = new Queue<ScheduledTask>();
        }

        public void Register(ScheduledTask task)
        {
            _scheduledTasks.Append(task);
        }

        public ScheduledTask Find(Guid id)
        {
            return _scheduledTasks.Where(s => s.Id == id).First();
        }

        public ScheduledTask GetNext()
        {
            return _scheduledTasks.Peek();
        }
        
        public IEnumerable<ScheduledTask> All()
        {
            return _scheduledTasks;
        }

        private readonly Queue<ScheduledTask> _scheduledTasks;
    }
}