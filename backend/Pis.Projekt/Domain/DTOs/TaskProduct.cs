namespace Pis.Projekt.Domain.DTOs
{
    public class TaskProduct : PricedProduct
    {
        public int SoldAmount { get; set; }
        public decimal SaleCoefficient { get; set; }
    }
}