using System;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class SalesAggregateEntity : IEntity<uint>
    {
        /// <summary>
        /// Code of week
        /// </summary>
        public uint Id { get; set; }
        public Guid PricedProductGuid { get; set; }
        public PricedProductEntity Product { get; set; }
        public decimal SaleCoefficient { get; set; }
    }
}