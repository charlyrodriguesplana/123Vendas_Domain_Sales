using MediatR;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.Cancel
{
    public record CancelSaleCommand(Guid SaleId) : IRequest;
}
