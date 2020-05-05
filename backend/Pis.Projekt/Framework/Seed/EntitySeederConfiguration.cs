using System.Collections.Generic;

namespace Pis.Projekt.Framework.Seed
{
    public class EntitySeederConfiguration
    {
        public bool SeedAtStart { get; set; }
        public uint WeekAmount { get; set; }
        public IEnumerable<string> ProductNames => ProductNamesRaw.Split(";");
        public string ProductNamesRaw { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
    }
}