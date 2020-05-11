using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class AdvertisedProductEntity : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Column("product_id")]
        public Guid ProductId { get; set; }
        [Column("week_number")]
        public uint WeekNumber { get; set; }

        [Column("name")]
        public string ProductName { get; set; }
    }
}