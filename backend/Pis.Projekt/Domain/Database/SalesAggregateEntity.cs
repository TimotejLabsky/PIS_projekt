using System;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class SalesAggregateEntity : IEntity<Guid>
    {
        /// <summary>
        /// Code of week
        /// </summary>
        public Guid Id { get; set; }
        public uint WeekNumber { get; set; }
        public Guid PricedProductGuid { get; set; }
        public PricedProductEntity Product { get; set; }
        public decimal SaleCoefficient { get; set; }
    }
}