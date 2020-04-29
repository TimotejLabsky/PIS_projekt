using System.Collections.Generic;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business.Scheduling.Impl
{
    public class ProductSalesDecreasedTask : IScheduledTask<IEnumerable<PricedProduct>>
    {
        public ProductSalesDecreasedTask(string name, IEnumerable<PricedProduct> products)
        {
            Name = name;
            Products = products;
        }

        public string Name { get; }
        public IEnumerable<PricedProduct> Result { get; set; }
        public IEnumerable<PricedProduct> Products { get; }
    }
}