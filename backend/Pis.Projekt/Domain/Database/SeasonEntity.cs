using System;
using System.ComponentModel.DataAnnotations.Schema;
using Pis.Projekt.Framework.Repositories;

namespace Pis.Projekt.Domain.Database
{
    public class SeasonEntity : IEntity<Guid>
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("start_at")]
        public DateTime StartAt { get; set; }

        [Column("end_at")]
        public DateTime EndAt { get; set; }
    }
}