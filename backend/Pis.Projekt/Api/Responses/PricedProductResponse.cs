using Newtonsoft.Json;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Responses
{
    [JsonObject]
    public class PricedProductResponse
    {
        [JsonProperty("sales_week")]
        public uint SalesWeek { get; set; }
        
        [JsonProperty("product")]
        public ProductResponse Product { get; set; }
        
        [JsonProperty("price")]
        public decimal Price { get; set; }
        
        [JsonProperty("currency")]
        public string Currency { get; set; }
        
    }
}