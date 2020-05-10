namespace Pis.Projekt.Domain.DTOs
{
    public class TaskProduct : PricedProduct
    {
        public int SoldAmount { get; set; }
        public decimal SaleCoefficient { get; set; }
        public bool IsAdvertised { get; set; }
        public bool IsCancelled { get; set; }
    }
}