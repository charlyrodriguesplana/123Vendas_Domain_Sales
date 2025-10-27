namespace _123Vendas.Domain.Sales.Application.UsesCase.Shared
{
    public record SaleItemDto(
       Guid ProductId,
       string ProductName,
       decimal UnitPrice,
       int Quantity
   );
}
