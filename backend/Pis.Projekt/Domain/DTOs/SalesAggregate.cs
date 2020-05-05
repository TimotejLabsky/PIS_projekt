using System;
using Pis.Projekt.Domain.Database;

namespace Pis.Projekt.Domain.DTOs
{
    public class SalesAggregate
    {
        public Guid Guid { get; set; }
        public Guid PricedProductGuid { get; set; }
        public PricedProduct PricedProduct { get; set; }
        public decimal SaleCoefficient { get; set; }
        
        public uint WeekNumber { get; set; }
    }
}