using System;
using Pis.Projekt.Domain.Database;

namespace Pis.Projekt.Domain.DTOs
{
    public class SalesAggregate
    {
        public Guid Id { get; set; }
        public Guid ProductGuid { get; set; }
        public Product Product { get; set; }
        public decimal SaleCoefficient { get; set; }
        
        public uint WeekNumber { get; set; }
        
        public decimal Price { get; set; }

    }
}