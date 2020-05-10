using System.Collections.Generic;

namespace Pis.Projekt.Domain.DTOs
{
    public class SeasonEvaluationResult
    {

        /// <summary>
        /// List od products where sales DECREASED by 20% or more
        /// </summary>
        public IEnumerable<SeasonTaskProduct> DecreasedSales { get; set; }
        public IEnumerable<SeasonPricedProduct> OtherSales { get; set; }
    }
}