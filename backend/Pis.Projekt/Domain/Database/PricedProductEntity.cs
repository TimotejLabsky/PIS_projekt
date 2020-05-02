using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    [Table("priced_products")]
    public class PricedProductEntity: IEntity<Guid>
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("sales_week")]
        public uint SalesWeek { get; set; }
        
        [Column("product_guid")]
        public Guid ProductGuid { get; set; }
        public ProductEntity Product { get; set; }
        
        [Column("price")]
        public decimal Price { get; set; }
        
        [Column("currency")] 
        public string Currency => "EUR";

    }
}