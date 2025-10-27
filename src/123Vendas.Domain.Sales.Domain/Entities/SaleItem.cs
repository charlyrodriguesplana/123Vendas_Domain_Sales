using _123Vendas.Domain.Sales.Domain.Exceptions;

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

        private SaleItem() { }

        public SaleItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            if (productId == Guid.Empty)
                throw new DomainException("Produto Inválido");

            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainException("Nome do produto é obrigatório");

            if (unitPrice <= 0)
                throw new DomainException("Valor unitário não deve ser zero");

            if (quantity <= 0)
                throw new DomainException("Quantidade precisa ser informada");

            if (quantity >= 20)
                throw new DomainException("Não é permitido a venda de 20 itens iguais");

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
