using System;
using System.Collections.Generic;
using Pis.Projekt.Domain.DTOs;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Business.Scheduling
{
    public class ScheduledTask : IEntity<int>
    {
        public ScheduledTask(int id, IEnumerable<PricedProduct> products, DateTime scheduledOn,
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
        public bool IsResolved { get; set; }
    }
    
    public class ScheduledTaskResult
    {
        public int TaskId { get; set; }
        public IEnumerable<PricedProduct> Products { get; set; }
    }
}