using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pis.Projekt.Domain.Database;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Responses
{
    [JsonObject]
    public class UserTaskResponse
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }
        [JsonProperty("task_type")]
        public UserTaskType Type { get; set; }
        [JsonProperty("sales")]
        public IEnumerable<SalesAggregateResponse> Sales { get; set; }
    }
}