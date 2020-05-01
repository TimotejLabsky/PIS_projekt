using System;
using Newtonsoft.Json;

namespace Pis.Projekt.Api.Responses
{
    [JsonObject]
    public class ProductResponse
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}