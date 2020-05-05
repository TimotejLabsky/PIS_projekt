using System.ComponentModel.DataAnnotations.Schema;

namespace Pis.Projekt.Domain.Database
{
    [Table("season_priced_products")]
    public class SeasonPricedProductEntity : PricedProductEntity
    {
        [Column("is_seasonal")]
        public bool IsSeasonal { get; set; }
    }
}