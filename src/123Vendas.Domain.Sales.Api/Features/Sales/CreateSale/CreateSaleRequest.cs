namespace _123Vendas.Domain.Sales.Api.Features.Sales.CreateSale
{
    public record CreateSaleRequest
    {
        public string SaleNumber { get; init; } = string.Empty;
        public Guid CustomerId { get; init; }

        public string CustomerName { get; init; } = string.Empty;

        public Guid BranchId { get; init; }
        public string BranchName { get; init; } = string.Empty;
        public List<CreateSaleItemRequest> Items { get; init; } = new();
    }

    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; init; }

        public string ProductName { get; init; } = string.Empty;
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
    }
}
