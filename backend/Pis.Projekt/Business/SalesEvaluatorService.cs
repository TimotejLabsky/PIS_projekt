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
            _logger.LogBusinessCase(BusinessTasks.SaleEvaluationTask);
            var decreasedSales = new List<TaskProduct>();
            var increasedSales = new List<KeyValuePair<PricedProduct,int>>();
            var sameSales = new List<PricedProduct>();

            foreach (var salesAggregate in allProducts)
            {
                if (salesAggregate.SaleCoefficient >= new decimal(1.1))
                {
                    increasedSales.Add(new KeyValuePair<PricedProduct, int>
                    (
                        new PricedProduct
                        {
                            Id = salesAggregate.ProductGuid,
                            Price = salesAggregate.Price,
                            Product = salesAggregate.Product,
                            SalesWeek = salesAggregate.WeekNumber
                        },salesAggregate.SoldAmount
                    ));
                } else if (salesAggregate.SaleCoefficient <= new decimal(0.8))
                {
                    decreasedSales.Add(new TaskProduct()
                    {
                        Id = salesAggregate.ProductGuid,
                        Price = salesAggregate.Price,
                        Product = salesAggregate.Product,
                        SalesWeek = salesAggregate.WeekNumber,
                        SaleCoefficient = salesAggregate.SaleCoefficient,
                        SoldAmount = salesAggregate.SoldAmount
                    });
                }
                else
                {
                    sameSales.Add(new PricedProduct
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
                DecreasedSales = decreasedSales,
                SameSales = sameSales
            };
        }

        private readonly ILogger<SalesEvaluatorService> _logger;
    }
}