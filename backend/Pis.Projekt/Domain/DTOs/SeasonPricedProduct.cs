using System;
using Pis.Projekt.Domain.Database;

namespace Pis.Projekt.Domain.DTOs
{
    public class SeasonPricedProduct
    {
        public Guid Id { get; set; }
        
        public Guid SeasonId { get; set; }
        
        public Guid PricedProductEntityId { get; set; }
        
        public decimal SaleCoefficient { get; set; }
        public PricedProductEntity PricedProductEntity { get; set; }
    }
}