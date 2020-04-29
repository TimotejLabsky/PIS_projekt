using System;
using System.ComponentModel.DataAnnotations;

namespace Pis.Projekt.Domain.Database
{
    public class PricedProductEntity
    {
        [Key]
        public uint SalesWeek { get; set; }
        public Guid ProductGuid { get; set; }
        public ProductEntity Product { get; set; }
        public decimal Price { get; set; }
        public string Currency => "EUR";
    }
}