namespace _123Vendas.Domain.Sales.Api.Features.Sales.CancelSale
{
    public class CancelSaleResponse
    {
        public Guid SaleId { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime CanceledAt { get; set; }
    }
}
