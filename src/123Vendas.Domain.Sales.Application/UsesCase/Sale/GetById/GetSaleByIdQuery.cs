using MediatR;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetById
{
    public record GetSaleByIdQuery(Guid Id) : IRequest<SaleDetailDto>;
}
