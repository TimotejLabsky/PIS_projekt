using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Business;
using Pis.Projekt.Domain.Repositories;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    [Table("sales_aggregates")]
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
        public Guid PricedProductGuid { get; set; }
        public PricedProductEntity PricedProduct { get; set; }
        
        [Column("sale_coefficient")]
        [Required]
        public decimal SaleCoefficient { get; set; }
    }
}