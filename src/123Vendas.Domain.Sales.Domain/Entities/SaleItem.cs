using _123Vendas.Domain.Sales.Domain.Exceptions;
using Sales.Domain.Core;

namespace _123Vendas.Domain.Sales.Domain.Entities
{
    public class SaleItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal DiscountPercent { get; private set; }
        public decimal TotalValue => CalculateTotal();

        private SaleItem() { } // for EF

        public SaleItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            if (productId == Guid.Empty)
                throw new DomainException("Invalid ProductId");

            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainException("Product name is required");

            if (unitPrice <= 0)
                throw new DomainException("Invalid unit price");

            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            if (quantity > 20)
                throw new DomainException("Cannot sell more than 20 identical items");

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            DiscountPercent = DefineDiscount(quantity);
        }

        private decimal DefineDiscount(int quantity)
        {
            if (quantity < 4) return 0m;
            if (quantity < 10) return 0.10m;
            return 0.20m;
        }

        private decimal CalculateTotal()
        {
            var gross = UnitPrice * Quantity;
            var discount = gross * DiscountPercent;
            return gross - discount;
        }
    }
}
