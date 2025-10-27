using _123Vendas.Domain.Sales.Domain.Repositories;
using MediatR;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetById
{
    public class GetSaleByIdQueryHandler(ISaleRepository saleRepository)
        : IRequestHandler<GetSaleByIdQuery, SaleDetailDto>
    {
        public async Task<SaleDetailDto> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = await saleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (sale == null)
                throw new InvalidOperationException($"Venda com ID {request.Id} não encontrada");

            return new SaleDetailDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                CustomerName = sale.CustomerName,
                BranchName = sale.BranchName,
                TotalValue = sale.TotalValue,
                Status = sale.Status.ToString(),
                Items = sale.Items.Select(item => new SaleItemDto
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    DiscountPercent = item.DiscountPercent,
                    TotalValue = item.TotalValue
                }).ToList()
            };
        }
    }
}
