using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business
{
    // Done
    public class SalesEvaluatorService
    {
        public SalesEvaluatorService(ILogger<SalesEvaluatorService> logger)
        {
            _logger = logger;
        }
        
        public EvaluationResult EvaluateSales(IEnumerable<SalesAggregate> allProducts)
        {
            _logger.LogBusinessCase(BusinessTasks.EvaluationTask);
            var decreasedSales = new List<PricedProduct>();
            var increasedSales = new List<PricedProduct>();

            foreach (var salesAggregate in allProducts)
            {
                if (salesAggregate.SaleCoefficient >= new decimal(1.1))
                {
                    increasedSales.Add(new PricedProduct
                    {
                        Id = salesAggregate.ProductGuid,
                        Price = salesAggregate.Price,
                        Product = salesAggregate.Product,
                        SalesWeek = salesAggregate.WeekNumber
                    });
                }
                
                if (salesAggregate.SaleCoefficient <= new decimal(0.8))
                {
                    decreasedSales.Add(new PricedProduct
                    {
                        Id = salesAggregate.ProductGuid,
                        Price = salesAggregate.Price,
                        Product = salesAggregate.Product,
                        SalesWeek = salesAggregate.WeekNumber
                    });
                }
            }
            
            return new EvaluationResult
            {
                IncreasedSales = increasedSales,
                DecreasedSales = decreasedSales
            };
        }

        private readonly ILogger<SalesEvaluatorService> _logger;
    }
}