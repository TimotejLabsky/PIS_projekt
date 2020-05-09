using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business
{
    public class IncreasedSalesHandler
    {
        public IncreasedSalesHandler(PriceCalculatorService priceCalculator)
        {
            _priceCalculator = priceCalculator;
        }
        
        public async Task<IEnumerable<PricedProduct>> Handle(IEnumerable<PricedProduct> decreasedList)
        {
            var pricedProducts = decreasedList.ToList();
            foreach (var product in pricedProducts)
            {
                product.SalesWeek++;
                product.Price = _priceCalculator.CalculatePrice(product);
            }
            await Task.CompletedTask;
            return pricedProducts;
        }

        private readonly PriceCalculatorService _priceCalculator;
    }
}