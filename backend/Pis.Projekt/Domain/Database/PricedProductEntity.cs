using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    [Table("priced_products")]
    public class PricedProductEntity: IEntity<Guid>
    {
        [Column("id")]
        [NotMapped]
        [Required]
        public Guid Id { get; set; }

        [Column("sales_week")]
        [Required]
        public uint SalesWeek { get; set; }
        
        [Column("product_guid")]
        [Required]
        public Guid ProductGuid { get; set; }
        public ProductEntity Product { get; set; }
        
        [Column("price")]
        [Required]
        public decimal Price { get; set; }
        
        [Column("currency")]
        [MaxLength(3)]
        public string Currency => "EUR";
        
        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

    }
}