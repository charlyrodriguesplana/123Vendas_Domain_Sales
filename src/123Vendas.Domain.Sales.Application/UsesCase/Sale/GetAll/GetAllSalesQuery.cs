using MediatR;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetAll
{
    public record GetAllSalesQuery : IRequest<List<SaleDto>>;
}
