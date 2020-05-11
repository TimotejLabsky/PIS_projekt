using System.Collections.Generic;

namespace Pis.Projekt.Domain.DTOs
{
    public class SeasonEvaluationResult
    {

        /// <summary>
        /// List od products where sales DECREASED by 20% or more
        /// </summary>
        public IEnumerable<TaskProduct> DecreasedSales { get; set; }
        public IEnumerable<TaskProduct> OtherSales { get; set; }
    }
}