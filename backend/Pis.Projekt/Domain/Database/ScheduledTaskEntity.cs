using System;
using System.Collections.Generic;

namespace Pis.Projekt.Domain.Database
{
    public class ScheduledTaskEntity
    {
        public int Id { get; set; }
        public IEnumerable<PricedProductEntity> Products { get; set; }
        public DateTime ScheduledOn { get; set; }
        public string Name { get; set; }
        public bool IsResolved { get; set; }
    }
}