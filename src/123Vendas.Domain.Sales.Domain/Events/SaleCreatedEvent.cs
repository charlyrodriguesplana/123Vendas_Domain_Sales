using MediatR;

namespace _123Vendas.Domain.Sales.Domain.Events
{
    /// <summary>
    /// The sale created event.
    /// </summary>
    public record SaleCreatedEvent(
         Guid SaleId,
         string SaleNumber,
         decimal TotalValue,
         DateTime SaleDate
     ) : INotification;
}
