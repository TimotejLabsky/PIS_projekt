using System.Collections.Generic;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business
{
    public class SalesEvaluatorService
    {
        public EvaluationResult EvaluateSales(IEnumerable<SalesAggregate> allProducts)
        {
            var decreasedSales = new List<PricedProduct>();
            var increasedSales = new List<PricedProduct>();

            foreach (var salesAggregate in allProducts)
            {
                if (salesAggregate.SaleCoefficient >= new decimal(1.1))
                {
                    increasedSales.Add(salesAggregate.PricedProduct);
                }
                
                if (salesAggregate.SaleCoefficient <= new decimal(0.8))
                {
                    decreasedSales.Add(salesAggregate.PricedProduct);
                }
            }
            
            return new EvaluationResult
            {
                IncreasedSales = increasedSales,
                DecreasedSales = decreasedSales
            };
        }
    }
}