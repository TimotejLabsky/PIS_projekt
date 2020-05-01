using Pis.Projekt.Domain.DTOs;

namespace Pis.Projekt.Business
{
    public class PriceCalculatorService
    {
        public decimal CalculatePrice(PricedProduct product)
        {
            return product.Price * new decimal(0.9);
        }
    }
}