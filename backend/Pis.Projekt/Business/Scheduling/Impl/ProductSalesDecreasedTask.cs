using System;
using System.Collections.Generic;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class ProductSalesDecreasedTask : ScheduledTask
    {
        public ProductSalesDecreasedTask(string name,
            IEnumerable<PricedProduct> products,
            DateTime scheduledOn) : base(products, scheduledOn, name)
        {
            Name = name;
            Products = products;
        }

        public string Name { get; }

        public IEnumerable<PricedProduct> Products { get; }
    }
}