using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pis.Projekt.Domain.DTOs;
using NotImplementedException = System.NotImplementedException;
// ReSharper disable All

namespace Pis.Projekt.Business
{
    public class IncreasedSalesHandler
    {
        public IncreasedSalesHandler(PriceCalculatorService priceCalculator, SupplierService supplier, ILogger<IncreasedSalesHandler> logger)
        {
            _priceCalculator = priceCalculator;
            _supplier = supplier;
            _logger = logger;
        }

        public async Task<IEnumerable<PricedProduct>> Handle(
            IEnumerable<KeyValuePair<PricedProduct, int>> decreasedList, Guid storeIdentification = default)
        {
            _logger.LogBusinessCase(BusinessTasks.IncreasedSalesBranch);
            var updatedPrice = CalculateFinalPrice(decreasedList);
            var order = CreateOrder(updatedPrice, storeIdentification);
            await _supplier.SendOrder(order).ConfigureAwait(false);
            return updatedPrice.Select(s=>s.Key);
        }

        private IEnumerable<KeyValuePair<PricedProduct, int>> CalculateFinalPrice(
            IEnumerable<KeyValuePair<PricedProduct, int>> decreasedList)
        {
            _logger.LogBusinessCase(BusinessTasks.CalculateFinalPrice);
            _logger.LogInput(BusinessTasks.CalculateFinalPrice
                ,"Zoznam produktov so zvýšenou predajnosťou"
                , decreasedList.Select(s=> s.Key));
            var pricedProducts = decreasedList.ToList();
            foreach (var product in pricedProducts)
            {
                product.Key.SalesWeek++;
                product.Key.Price = _priceCalculator.CalculatePrice(product.Key);
            }
            _logger.LogOutput(BusinessTasks.CalculateFinalPrice,"Zoznam produktov s novou cenou"
            , pricedProducts.Select(s=>s.Key));
            return pricedProducts;
        }

        private ProductOrder CreateOrder(IEnumerable<KeyValuePair<PricedProduct, int>> updatedPrice,
            Guid storeIdentification)
        {
            _logger.LogBusinessCase(BusinessTasks.CreateOrder);
            _logger.LogInput(BusinessTasks.CreateOrder, "Zoznam produktov s novou cenou"
                , updatedPrice.Select(s=>s.Key));
            
            var order = new ProductOrder
            {
                Guid = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Products = updatedPrice.Select(s => new KeyValuePair<Guid, int>(s.Key.Id, s.Value)),
                StoreIdentification = storeIdentification
            };
            _logger.LogOutput(BusinessTasks.CreateOrder, "Objednávka", order);
            return order;
        }

        private async Task SendOrder(ProductOrder order)
        {
            _logger.LogBusinessCase(BusinessTasks.SendOrder);
            _logger.LogInput(BusinessTasks.SendOrder, "Objednávka", order);
            await _supplier.SendOrder(order).ConfigureAwait(false);
        }


        private readonly PriceCalculatorService _priceCalculator;
        private readonly SupplierService _supplier;
        private readonly ILogger<IncreasedSalesHandler> _logger;
    }
}