using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Business.Scheduling
{
    public class ScheduledTask : IEntity<int>, ITask<IEnumerable<PricedProduct>>
    {
        public ScheduledTask(int id,
            IEnumerable<PricedProduct> products,
            DateTime scheduledOn,
            string name)
        {
            Id = id;
            Products = products;
            ScheduledOn = scheduledOn;
            Name = name;
            IsResolved = false;
        }

        //TODO JsonProperty attributes
        public int Id { get; set; }
        public IEnumerable<PricedProduct> Products { get; set; }
        public DateTime ScheduledOn { get; set; }
        public string Name { get; set; }
        public IEnumerable<PricedProduct> Result { get; set; }
        public bool IsResolved { get; set; }

        public void Evaluate()
        {
            if (Result == null)
            {
                OnTaskFailed?.Invoke(this);
            }
        }

        public void Fulfill(IEnumerable<PricedProduct> result)
        {
            Result = result;
            OnTaskFulfilled?.Invoke(new ScheduledTaskResult {Products = Result});
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
        public IEnumerable<PricedProduct> Products { get; set; }
    }
}