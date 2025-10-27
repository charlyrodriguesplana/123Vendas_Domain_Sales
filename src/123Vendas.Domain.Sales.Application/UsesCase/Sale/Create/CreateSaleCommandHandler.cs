using _123Vendas.Domain.Sales.Domain.Repositories;
using _123Vendas.Domain.Sales.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using SaleEntity = _123Vendas.Domain.Sales.Domain.Entities.Sale;
using SaleItemEntity = _123Vendas.Domain.Sales.Domain.Entities.SaleItem;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create
{
    public class CreateSaleCommandHandler
            (
                ISaleRepository _saleRepository,
                IMediator _mediator,
                ILogger<CreateSaleCommandHandler> _logger
            ) : IRequestHandler<CreateSaleCommand, Guid>
    {

        public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            
            var items = request.Items
                .Select(i => new SaleItemEntity(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity))
                .ToList();

            var customerSnapshot = new Customer(request.CustomerId, request.CustomerName);
            var branchSnapshot = new Branch(request.BranchId, request.BranchName);

            var sale = SaleEntity.Create(
                saleNumber: request.SaleNumber,
                saleDate: DateTime.UtcNow,
                customer: customerSnapshot,
                branch: branchSnapshot,
                items: items
            );

            await _saleRepository.AddAsync(sale, cancellationToken);
            await _saleRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Venda criada com sucesso com o ID: {SaleId}", sale.Id);

            var eventTasks = sale.Events.Select(@event => _mediator.Publish(@event, cancellationToken));
            await Task.WhenAll(eventTasks);


            sale.ClearEvents();

            return sale.Id;
        }
    }
}
