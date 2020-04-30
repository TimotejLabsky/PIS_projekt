using System;

namespace Pis.Projekt.Domain.DTOs
{
    public class PricedProduct
    {
        public uint SalesWeek { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}