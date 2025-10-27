using MediatR;

namespace _123Vendas.Domain.Sales.Domain.Events
{
    /// <summary>
    /// The sale canceled event.
    /// </summary>
    public record SaleCanceledEvent(Guid SaleId, string SaleNumber, DateTime CanceledAt) : INotification;
}
