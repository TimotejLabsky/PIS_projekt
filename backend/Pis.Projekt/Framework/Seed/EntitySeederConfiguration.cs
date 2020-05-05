using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace Pis.Projekt.Framework.Seed
{
    public class EntitySeederConfiguration
    {
        public bool SeedAtStart { get; set; }
        public uint WeekAmount { get; set; }
        public string ProductsCSV { get; set; }

        public IEnumerable<string> ProductNames => ProductsCSV.Split(";");
        public double PriceMin { get; set; }
        public double PriceMax { get; set; }
        
        public double SalesCoefMin { get; set; }
        public double SalesCoefMax { get; set; }
    }
}