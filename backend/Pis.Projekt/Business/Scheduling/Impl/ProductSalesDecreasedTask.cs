using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class ProductSalesDecreasedTask : ISchedulableTask<IEnumerable<PricedProduct>>
    {
        public ProductSalesDecreasedTask(string name, IEnumerable<PricedProduct> products)
        {
            Name = name;
            Products = products;
        }

        public string Name { get; }

        public ScheduledTask Schedule(int id)
        {
            return new ScheduledTask(id, Products, DateTime.Now, Name);
        }
        public IEnumerable<PricedProduct> Products { get; }
    }
}