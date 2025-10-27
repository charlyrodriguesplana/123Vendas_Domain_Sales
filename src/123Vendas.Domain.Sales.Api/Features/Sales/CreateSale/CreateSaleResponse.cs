namespace _123Vendas.Domain.Sales.Api.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal TotalValue { get; set; }
        public int ItemsCount { get; set; }
    }
}
