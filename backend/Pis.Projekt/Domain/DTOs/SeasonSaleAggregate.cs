using System;

namespace Pis.Projekt.Domain.DTOs
{
    public class SeasonSaleAggregate : SalesAggregate
    {
        public bool IsSeasonal { get; set; }
        public Guid SeasonGuid { get; set; }
    }
}