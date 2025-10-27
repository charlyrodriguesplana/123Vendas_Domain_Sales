using _123Vendas.Domain.Sales.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace _123Vendas.Domain.Sales.Application.Events
{
    public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
    {
        private readonly ILogger<SaleCreatedEventHandler> _logger;

        public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleCreatedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Execução do evento de venda criada: {event}", @event);

            return Task.CompletedTask;
        }
    }
}
