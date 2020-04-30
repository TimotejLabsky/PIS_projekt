using System;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class ProductEntity: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}