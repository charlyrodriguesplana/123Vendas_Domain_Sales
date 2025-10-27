using _123Vendas.Domain.Sales.Domain.Enums;
using _123Vendas.Domain.Sales.Domain.Events;
using _123Vendas.Domain.Sales.Domain.Exceptions;
using _123Vendas.Domain.Sales.Domain.ValueObjects;
using MediatR;

namespace _123Vendas.Domain.Sales.Domain.Entities
{
    public class Sale
    {
        private readonly List<SaleItem> _items = new();
        private readonly List<INotification> _events = new();

        public Guid Id { get; private set; }
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }

        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public Guid BranchId { get; private set; }
        public string BranchName { get; private set; }

        public SaleStatus Status { get; private set; }

        public IEnumerable<SaleItem> Items => _items;
        public decimal TotalValue => _items.Sum(i => i.TotalValue);
        public IEnumerable<INotification> Events => _events;

        private Sale() { }

        private Sale(
            string saleNumber,
            DateTime saleDate,
            Customer customer,
            Branch branch,
            IEnumerable<SaleItem> items)
        {
            if (string.IsNullOrWhiteSpace(saleNumber))
                throw new DomainException("Número da venda é obrigatório");

            SaleNumber = saleNumber;
            SaleDate = saleDate;

            CustomerId = customer.Id;
            CustomerName = customer.Name;

            BranchId = branch.Id;
            BranchName = branch.Name;

            Status = SaleStatus.Active;
            _items.AddRange(items);

            if (_items.Count == 0)
                throw new DomainException("A venda deve conter pelo menos um item");
        }

        public static Sale Create(
            string saleNumber,
            DateTime saleDate,
            Customer customer,
            Branch branch,
            IEnumerable<SaleItem> items)
        {
            var sale = new Sale(saleNumber, saleDate, customer, branch, items)
            {
                Id = Guid.NewGuid(),
                Status = SaleStatus.Active
            };

            sale.RegisterEvent(new SaleCreatedEvent(
                sale.Id,
                sale.SaleNumber,
                sale.TotalValue,
                sale.SaleDate
            ));

            return sale;
        }

        public void CancelSale()
        {
            if (Status == SaleStatus.Cancelled)
                return;

            Status = SaleStatus.Cancelled;

            RegisterEvent(new SaleCanceledEvent(
                Id,
                SaleNumber,
                DateTime.UtcNow
            ));
        }

        public void CancelItem(Guid productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new DomainException("Item não encontrado para essa venda.");

            //Poderiamos ter um evento para item cancelado também.
        }

        private void RegisterEvent(INotification @event)
            => _events.Add(@event);

        public void ClearEvents() => _events.Clear();
    }
}
