using _123Vendas.Domain.Sales.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _123Vendas.Domain.Sales.Application.Events
{
    public class SaleCanceledEventHandler(ILogger<SaleCanceledEventHandler> logger)
        : INotificationHandler<SaleCanceledEvent>
    {
        public Task Handle(SaleCanceledEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Execução do evento de venda cancelada: {notification}", notification);

            return Task.CompletedTask;
        }
    }
}
