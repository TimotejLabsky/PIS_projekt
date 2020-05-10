using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    [Table("season_priced_products")]
    public class SeasonPricedProductEntity : IEntity<Guid>
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("season_id")]
        public Guid SeasonId { get; set; }
        
        [Column("priced_product_id")]
        public Guid PricedProductEntityId { get; set; }
        public PricedProductEntity PricedProductEntity { get; set; }
        
    }
}