using _123Vendas.Domain.Sales.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _123Vendas.Domain.Sales.Application.UsesCase.Sale.Cancel
{
    public class CancelSaleCommandHandler(
        ISaleRepository saleRepository,
        IMediator mediator,
        ILogger<CancelSaleCommandHandler> logger) : IRequestHandler<CancelSaleCommand>
    {
        public async Task Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await saleRepository.GetByIdAsync(request.SaleId, cancellationToken);

            if (sale == null)
                throw new InvalidOperationException($"Venda com ID {request.SaleId} não encontrada");

            sale.CancelSale();

            await saleRepository.UpdateAsync(sale, cancellationToken);
            await saleRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Venda cancelada com sucesso. SaleId: {SaleId}", sale.Id);

            var eventTasks = sale.Events.Select(@event => mediator.Publish(@event, cancellationToken));
            await Task.WhenAll(eventTasks);

            sale.ClearEvents();
        }
    }
}
