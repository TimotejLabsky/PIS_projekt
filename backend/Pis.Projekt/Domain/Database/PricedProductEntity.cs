using System;
using System.ComponentModel.DataAnnotations;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class PricedProductEntity: IEntity<Guid>
    {
        public Guid Id { get; }

        public uint SalesWeek { get; set; }
        public Guid ProductGuid { get; set; }
        public ProductEntity Product { get; set; }
        public decimal Price { get; set; }
        public string Currency => "EUR";
    }
}