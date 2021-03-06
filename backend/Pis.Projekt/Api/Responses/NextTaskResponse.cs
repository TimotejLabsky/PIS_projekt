using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pis.Projekt.Api.Responses
{
    [JsonObject]
    public class NextTaskResponse
    {
        [JsonProperty("guid")]
        public Guid Id { get; set; }

        [JsonProperty("taskType")]
        public string Name { get; set; }

        [JsonProperty("products")]
        public IEnumerable<TaskProductResponse> Products { get; set; }

        [JsonProperty("scheduledOn")]
        public DateTime ScheduledOn { get; set; }

        public class TaskProductResponse
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
            public decimal NewPrice => Price;

            [JsonProperty("currency")]
            public string Currency => "EUR";

            [JsonProperty("delta_sales")]
            public decimal SaleCoefficient { get; set; }

            [JsonProperty("sales")]
            public int SoldAmount { get; set; }
        }
    }
}