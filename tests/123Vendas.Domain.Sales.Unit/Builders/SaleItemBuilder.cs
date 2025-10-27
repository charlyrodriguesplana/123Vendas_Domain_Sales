using _123Vendas.Domain.Sales.Domain.Entities;
using Bogus;

namespace _123Vendas.Domain.Sales.Unit.Builders
{
    public class SaleItemBuilder
    {
        private static readonly Faker Faker = new();

        private Guid _productId = Guid.NewGuid();
        private string _productName = Faker.Commerce.ProductName();
        private decimal _unitPrice = Faker.Random.Decimal(10m, 1000m);
        private int _quantity = Faker.Random.Int(1, 10);

        public SaleItemBuilder WithProductId(Guid productId)
        {
            _productId = productId;
            return this;
        }

        public SaleItemBuilder WithProductName(string productName)
        {
            _productName = productName;
            return this;
        }

        public SaleItemBuilder WithUnitPrice(decimal unitPrice)
        {
            _unitPrice = unitPrice;
            return this;
        }

        public SaleItemBuilder WithQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public SaleItemBuilder WithEmptyProductId()
        {
            _productId = Guid.Empty;
            return this;
        }

        public SaleItemBuilder WithNullProductName()
        {
            _productName = null!;
            return this;
        }

        public SaleItemBuilder WithZeroUnitPrice()
        {
            _unitPrice = 0m;
            return this;
        }

        public SaleItemBuilder WithNegativeUnitPrice()
        {
            _unitPrice = -10m;
            return this;
        }

        public SaleItemBuilder WithZeroQuantity()
        {
            _quantity = 0;
            return this;
        }

        public SaleItemBuilder WithQuantity20OrMore()
        {
            _quantity = 20;
            return this;
        }

        public SaleItem Build()
        {
            return new SaleItem(_productId, _productName, _unitPrice, _quantity);
        }

        public static SaleItem BuildValid() => new SaleItemBuilder().Build();
    }
}
