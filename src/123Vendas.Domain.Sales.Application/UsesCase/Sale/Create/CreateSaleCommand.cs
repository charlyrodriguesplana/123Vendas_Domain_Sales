using _123Vendas.Domain.Sales.Application.UsesCase.Shared;
using MediatR;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create
{
    public record CreateSaleCommand(
       string SaleNumber,
       Guid CustomerId,
       string CustomerName,
       Guid BranchId,
       string BranchName,
       IEnumerable<SaleItemDto> Items
   ) : IRequest<Guid>;
}
