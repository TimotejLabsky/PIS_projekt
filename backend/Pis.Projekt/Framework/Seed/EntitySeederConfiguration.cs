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
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
    }
}