using System.Collections.Generic;

namespace Pis.Projekt.Domain.DTOs
{
    public class EvaluationResult
    {
        /// <summary>
        /// List of products where sales INCREASED by 10% or more
        /// </summary>
        public IEnumerable<KeyValuePair<PricedProduct,int>> IncreasedSales { get; set; }

        /// <summary>
        /// List od products where sales DECREASED by 20% or more
        /// </summary>
        public IEnumerable<TaskProduct> DecreasedSales { get; set; }
        public IEnumerable<PricedProduct> SameSales { get; set; }
    }
}