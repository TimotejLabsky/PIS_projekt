using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pis.Projekt.Api.Requests
{
    public class FulfillTaskRequest
    {
        [JsonProperty("guid")]
        public Guid Id { get; }

        [JsonProperty("taskType")]
        public string Name { get; set; }

        [JsonProperty("products")]
        public IEnumerable<PricedProductRequest> Products { get; set; }

        [JsonProperty("scheduledOn")]
        public DateTime ScheduledOn { get; set; }
        
        public class PricedProductRequest
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }

            [JsonProperty("product_id")]
            public Guid ProductId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("week_number")]
            public uint SalesWeek { get; set; }

            [JsonProperty("price")]
            public decimal Price { get; set; }

            [JsonProperty("new_price")]
            public decimal NewPrice { get; set; }

            [JsonProperty("currency")]
            public string Currency => "EUR";

            [JsonProperty("sales")]
            public decimal Sales { get; set; }
        }
    }
}