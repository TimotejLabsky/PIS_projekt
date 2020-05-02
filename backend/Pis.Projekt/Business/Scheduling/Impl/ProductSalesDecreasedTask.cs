using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class ProductSalesDecreasedTask : ISchedulabletask<IEnumerable<PricedProduct>>
    {
        public ProductSalesDecreasedTask(string name, IEnumerable<PricedProduct> products)
        {
            Name = name;
            Products = products;
        }

        public void Evaluate()
        {
            if (Result == null)
            {
                OnTaskFailed?.Invoke();
            }
        }

        public void Fulfill()
        {
            OnTaskFulfilled?.Invoke(this);
        }

        public string Name { get; }
        public IEnumerable<PricedProduct> Result { get; set; }

        public ScheduledTask Schedule(int id)
        {
            return new ScheduledTask
            {
                Id = id,
                Name = Name,
                Products = Products,
                ScheduledOn = DateTime.Now
            };
        }

        public event ISchedulabletask<IEnumerable<PricedProduct>>.TaskFulfilledHandler
            OnTaskFulfilled;

        public event ISchedulabletask<IEnumerable<PricedProduct>>.TaskFailedHandler OnTaskFailed;
        public IEnumerable<PricedProduct> Products { get; }
    }
}