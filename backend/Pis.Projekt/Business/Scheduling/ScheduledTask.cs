using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Business.Scheduling
{
    public class ScheduledTask : IEntity<Guid>, ITask<IEnumerable<PricedProduct>>
    {
        public ScheduledTask(
            Guid guid,
            IEnumerable<TaskProduct> products,
            DateTime scheduledOn,
            string name)
        {
            Products = products;
            ScheduledOn = scheduledOn;
            Name = name;
            IsResolved = false;
            Id = guid;
        }

        public Guid Id { get; }
        public int TaskId { get; }
        public IEnumerable<TaskProduct> Products { get; set; }
        public DateTime ScheduledOn { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaskProduct> Result { get; set; }
        public bool IsResolved { get; set; }
        

        public void Evaluate()
        {
            if (Result == null)
            {
                OnTaskFailed?.Invoke(this);
            }
        }

        public void Fulfill(IEnumerable<TaskProduct> result)
        {
            Result = result;
            OnTaskFulfilled?.Invoke(new ScheduledTaskResult {Products = Result, Name = Name});
            IsResolved = true;
        }
        
        public event TaskFulfilledHandler OnTaskFulfilled;
        public event TaskFailedHandler OnTaskFailed;

        public delegate Task TaskFulfilledHandler(ScheduledTaskResult result);

        public delegate Task TaskFailedHandler(ScheduledTask failedTask);
    }

    public class ScheduledTaskResult
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaskProduct> Products { get; set; }
    }
}