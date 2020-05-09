using System;
using System.Collections.Generic;

namespace Pis.Projekt.Domain.DTOs
{
    public class ProductOrder
    {
        public Guid Guid { get; set; }
        public DateTime CreatedAt { get; set; }
        private IEnumerable<KeyValuePair<Guid,int>> Products { get; set; }
        public Guid StoreIdentification { get; set; }
    }
}