using _123Vendas.Domain.Sales.Domain.Entities;
using _123Vendas.Domain.Sales.Domain.Repositories;
using _123Vendas.Domain.Sales.Domain.ValueObjects;
using MediatR;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create
{
    public class CreateSaleCommandHandler
        (
            ISaleRepository _saleRepository,
            IMediator _mediator
        ) : IRequestHandler<CreateSaleCommand, Guid>
    {

        public async Task<Guid> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var items = request.Items
                .Select(i => new SaleItem(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity))
                .ToList();

            var customerSnapshot = new Customer(request.CustomerId, request.CustomerName);
            var branchSnapshot = new Branch(request.BranchId, request.BranchName);

            var sale = Sale.Create(
                saleNumber: request.SaleNumber,
                saleDate: DateTime.UtcNow,
                customer: customerSnapshot,
                branch: branchSnapshot,
                items: items
            );

            await _saleRepository.AddAsync(sale, cancellationToken);
            await _saleRepository.SaveChangesAsync(cancellationToken);

            //Eu decidi registrar o evento direto na classe de dominio, eu poderia trazer isso para o handle também, tomei essa decisão com intuito de mostrar as possiblidade.
            //Aqui vamos ter o evento de venda criada
            sale.Events.Select(async _ => await _mediator.Publish(_, cancellationToken)).ToList();

            sale.ClearEvents();

            return sale.Id;
        }
    }
}
