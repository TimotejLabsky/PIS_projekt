using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pis.Projekt.Domain.DTOs;
using NotImplementedException = System.NotImplementedException;
// ReSharper disable All

namespace Pis.Projekt.Business
{
    public class IncreasedSalesHandler
    {
        public IncreasedSalesHandler(PriceCalculatorService priceCalculator, SupplierService supplier)
        {
            _priceCalculator = priceCalculator;
            _supplier = supplier;
        }

        public async Task<IEnumerable<PricedProduct>> Handle(
            IEnumerable<KeyValuePair<PricedProduct, int>> decreasedList, Guid storeIdentification = default)
        {
            var updatedPrice = CalculateFinalPrice(decreasedList);
            var order = CreateOrder(updatedPrice, storeIdentification);
            await _supplier.SendOrder(order).ConfigureAwait(false);
            return updatedPrice.Select(s=>s.Key);
        }

        private IEnumerable<KeyValuePair<PricedProduct, int>> CalculateFinalPrice(
            IEnumerable<KeyValuePair<PricedProduct, int>> decreasedList)
        {
            var pricedProducts = decreasedList.ToList();
            foreach (var product in pricedProducts)
            {
                product.Key.SalesWeek++;
                product.Key.Price = _priceCalculator.CalculatePrice(product.Key);
            }

            return pricedProducts;
        }

        private ProductOrder CreateOrder(IEnumerable<KeyValuePair<PricedProduct, int>> updatedPrice,
            Guid storeIdentification)
        {
            var order = new ProductOrder
            {
                Guid = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Products = updatedPrice.Select(s => new KeyValuePair<Guid, int>(s.Key.Id, s.Value)),
                StoreIdentification = storeIdentification
            };
            return order;
        }

        private async Task SendOrder(ProductOrder order)
        {
            await _supplier.SendOrder(order).ConfigureAwait(false);
        }


        private readonly PriceCalculatorService _priceCalculator;
        private readonly SupplierService _supplier;
    }
}