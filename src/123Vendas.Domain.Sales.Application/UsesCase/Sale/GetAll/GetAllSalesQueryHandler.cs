using _123Vendas.Domain.Sales.Domain.Repositories;
using MediatR;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetAll
{
    public class GetAllSalesQueryHandler(ISaleRepository saleRepository)
        : IRequestHandler<GetAllSalesQuery, List<SaleDto>>
    {
        public async Task<List<SaleDto>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await saleRepository.GetAllAsync(cancellationToken);

            return sales.Select(sale => new SaleDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                CustomerName = sale.CustomerName,
                BranchName = sale.BranchName,
                TotalValue = sale.TotalValue,
                Status = sale.Status.ToString()
            }).ToList();
        }
    }
}
