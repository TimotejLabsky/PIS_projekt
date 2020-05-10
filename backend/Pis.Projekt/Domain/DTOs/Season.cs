using System;
using System.Collections.Generic;

namespace Pis.Projekt.Domain.DTOs
{
    public class Season
    {
        public Guid Id { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}