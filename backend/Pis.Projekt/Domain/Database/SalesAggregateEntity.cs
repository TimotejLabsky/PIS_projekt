using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class SalesAggregateEntity : IEntity<Guid>
    {
        /// <summary>
        /// Code of week
        /// </summary>
        
        [Column("id")]
        [Required]
        public Guid Id { get; set; }
        
        [Column("week_number")]
        [Required]
        public uint WeekNumber { get; set; }
        
        [Column("priced_product_guid")]
        [Required]
        public Guid ProductGuid { get; set; }
        public ProductEntity Product { get; set; }
        
        [Column("sale_coefficient")]
        [Required]
        public decimal SaleCoefficient { get; set; }
        
        [Column("price")]
        [Required]
        public decimal Price { get; set; }
        
        [Column("sold_amount")]
        [Required]
        public int SoldAmount { get; set; }
        
    }
}