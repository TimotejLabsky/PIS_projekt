using System;
using Newtonsoft.Json;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Responses
{
    [JsonObject]
    public class SalesAggregateResponse
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }
        
        [JsonProperty("priced_product_guid")]
        public Guid PricedProductGuid { get; set; }
        
        [JsonProperty("priced_product")]
        public PricedProductResponse PricedProduct { get; set; }
        
        [JsonProperty("sale_coeficient")]
        public decimal SaleCoefficient { get; set; }
    }
}