using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    [Table("products")]
    public class ProductEntity: IEntity<Guid>
    {
        [Column("id")]
        public Guid Id { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
    }
}