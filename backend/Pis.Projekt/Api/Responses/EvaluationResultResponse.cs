using System.Collections.Generic;
using Newtonsoft.Json;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Api.Responses
{
    [JsonObject]
    public class EvaluationResultResponse
    {
        /// <summary>
        /// List of products where sales INCREASED by 10% or more
        /// </summary>
        [JsonProperty("increased_sales")]
        public IEnumerable<PricedProductResponse> IncreasedSales { get; set; }

        /// <summary>
        /// List od products where sales DECREASED by 20% or more
        /// </summary>
        [JsonProperty("decreased_sales")]
        public IEnumerable<PricedProductResponse> DecreasedSales { get; set; }
    }
}