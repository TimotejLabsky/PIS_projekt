using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;
using NotImplementedException = System.NotImplementedException;

namespace Pis.Projekt.Business
{
    public class IncreasedSalesHandler
    {
        public IncreasedSalesHandler(PriceCalculatorService priceCalculator, SupplierService supplier)
        {
            _priceCalculator = priceCalculator;
            _supplier = supplier;
        }
        
        public async Task<IEnumerable<PricedProduct>> Handle(IEnumerable<PricedProduct> decreasedList)
        {
            var updatedPrice =CalculateFinalPrice(decreasedList);
            var order = CreateOrder(updatedPrice);
        }

        private IEnumerable<PricedProduct> CalculateFinalPrice(IEnumerable<PricedProduct> decreasedList)
        {
            var pricedProducts = decreasedList.ToList();
            foreach (var product in pricedProducts)
            {
                product.SalesWeek++;
                product.Price = _priceCalculator.CalculatePrice(product);
            }
            return pricedProducts;
        }
        
        private ProductOrder CreateOrder(IEnumerable<PricedProduct> updatedPrice)
        {
            throw new NotImplementedException();
        }
        
        private async Task SendOrder(ProductOrder order)
        {
            await _supplier.SendOrder(order).ConfigureAwait(false);
        }
        

        private readonly PriceCalculatorService _priceCalculator;
        private readonly SupplierService _supplier;
    }
}