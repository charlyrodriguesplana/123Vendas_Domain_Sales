namespace _123Vendas.Domain.Sales.Api.Features.Sales.CancelSale
{
    public record CancelSaleRequest
    {
        public Guid SaleId { get; init; }
    }
}
