using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    [Table("products")]
    public class ProductEntity: IEntity<Guid>
    {
        [Column("id")]
        [Required]
        public Guid Id { get; set; }
        
        [Column("name")]
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
    }
}