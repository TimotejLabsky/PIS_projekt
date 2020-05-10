using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pis.Projekt.Api.Requests
{
    [JsonObject]
    public class TaskFulfillRequest
    {
        [JsonProperty("guid")]
        public Guid Id { get; set; }

        [JsonProperty("taskType")]
        public string Name { get; set; }

        [JsonProperty("products")]
        public IEnumerable<TaskProductRequest> Products { get; set; }

        [JsonProperty("scheduledOn")]
        public DateTime ScheduledOn { get; set; }

        public class TaskProductRequest
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

            [JsonProperty("delta_sales")]
            public decimal SaleCoefficient { get; set; }

            [JsonProperty("sales")]
            public int SoldAmount { get; set; }
            
            [JsonProperty("include_to_ad")]
            public bool IsAdvertised { get; set; }

            [JsonProperty("cancel_ordering")]
            public bool IsCancelled { get; set; }
        }
    }
}