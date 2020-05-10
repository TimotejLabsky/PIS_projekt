namespace Pis.Projekt.Domain.DTOs
{
    public class SeasonTaskProduct : SeasonPricedProduct
    {
        public int SoldAmount { get; set; }
        public decimal SaleCoefficient { get; set; }
        public bool IsAdvertised { get; set; }
        public bool IsCancelled { get; set; }
    }
}