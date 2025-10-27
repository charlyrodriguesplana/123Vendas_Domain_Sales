using _123Vendas.Domain.Sales.Domain.Exceptions;
using _123Vendas.Domain.Sales.Unit.Builders;
using Shouldly;
using Xunit;

namespace _123Vendas.Domain.Sales.Unit.Entities
{
    public class SaleItemTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateSaleItem()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productName = "Product Name";
            var unitPrice = 100m;
            var quantity = 5;

            // Act
            var saleItem = new SaleItemBuilder()
                .WithProductId(productId)
                .WithProductName(productName)
                .WithUnitPrice(unitPrice)
                .WithQuantity(quantity)
                .Build();

            // Assert
            saleItem.ShouldNotBeNull();
            saleItem.ProductId.ShouldBe(productId);
            saleItem.ProductName.ShouldBe(productName);
            saleItem.UnitPrice.ShouldBe(unitPrice);
            saleItem.Quantity.ShouldBe(quantity);
        }

        [Fact]
        public void Constructor_WithEmptyProductId_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithEmptyProductId()
                    .Build())
                .Message.ShouldBe("Produto Inválido");
        }

        [Fact]
        public void Constructor_WithNullProductName_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithNullProductName()
                    .Build())
                .Message.ShouldBe("Nome do produto é obrigatório");
        }

        [Fact]
        public void Constructor_WithEmptyProductName_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithProductName("")
                    .Build())
                .Message.ShouldBe("Nome do produto é obrigatório");
        }

        [Fact]
        public void Constructor_WithWhitespaceProductName_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithProductName("   ")
                    .Build())
                .Message.ShouldBe("Nome do produto é obrigatório");
        }

        [Fact]
        public void Constructor_WithZeroUnitPrice_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithZeroUnitPrice()
                    .Build())
                .Message.ShouldBe("Valor unitário não deve ser zero");
        }

        [Fact]
        public void Constructor_WithNegativeUnitPrice_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithNegativeUnitPrice()
                    .Build())
                .Message.ShouldBe("Valor unitário não deve ser zero");
        }

        [Fact]
        public void Constructor_WithZeroQuantity_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithZeroQuantity()
                    .Build())
                .Message.ShouldBe("Quantidade precisa ser informada");
        }

        [Fact]
        public void Constructor_WithNegativeQuantity_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithQuantity(-1)
                    .Build())
                .Message.ShouldBe("Quantidade precisa ser informada");
        }

        [Fact]
        public void Constructor_WithQuantity20OrMore_ShouldThrowDomainException()
        {
            // Arrange, Act, Assert
            Should.Throw<DomainException>(() =>
                new SaleItemBuilder()
                    .WithQuantity20OrMore()
                    .Build())
                .Message.ShouldBe("Não é permitido a venda de 20 itens iguais");
        }

        [Fact]
        public void DefineDiscount_WithQuantityLessThan4_ShouldReturn0Percent()
        {
            // Arrange, Act
            var saleItem = new SaleItemBuilder()
                .WithQuantity(3)
                .Build();

            // Assert
            saleItem.DiscountPercent.ShouldBe(0m);
        }

        [Fact]
        public void DefineDiscount_WithQuantity4_ShouldReturn10Percent()
        {
            // Arrange, Act
            var saleItem = new SaleItemBuilder()
                .WithQuantity(4)
                .Build();

            // Assert
            saleItem.DiscountPercent.ShouldBe(0.10m);
        }

        [Fact]
        public void DefineDiscount_WithQuantityBetween4And9_ShouldReturn10Percent()
        {
            // Arrange, Act
            var saleItem = new SaleItemBuilder()
                .WithQuantity(7)
                .Build();

            // Assert
            saleItem.DiscountPercent.ShouldBe(0.10m);
        }

        [Fact]
        public void DefineDiscount_WithQuantity9_ShouldReturn10Percent()
        {
            // Arrange, Act
            var saleItem = new SaleItemBuilder()
                .WithQuantity(9)
                .Build();

            // Assert
            saleItem.DiscountPercent.ShouldBe(0.10m);
        }

        [Fact]
        public void DefineDiscount_WithQuantity10_ShouldReturn20Percent()
        {
            // Arrange, Act
            var saleItem = new SaleItemBuilder()
                .WithQuantity(10)
                .Build();

            // Assert
            saleItem.DiscountPercent.ShouldBe(0.20m);
        }

        [Fact]
        public void DefineDiscount_WithQuantity19_ShouldReturn20Percent()
        {
            // Arrange, Act
            var saleItem = new SaleItemBuilder()
                .WithQuantity(19)
                .Build();

            // Assert
            saleItem.DiscountPercent.ShouldBe(0.20m);
        }

        [Fact]
        public void CalculateTotal_WithNoDiscount_ShouldReturnGrossValue()
        {
            // Arrange
            var unitPrice = 100m;
            var quantity = 2;

            // Act
            var saleItem = new SaleItemBuilder()
                .WithUnitPrice(unitPrice)
                .WithQuantity(quantity)
                .Build();

            // Assert
            saleItem.TotalValue.ShouldBe(200m);
        }

        [Fact]
        public void CalculateTotal_With10PercentDiscount_ShouldApplyDiscountCorrectly()
        {
            // Arrange
            var unitPrice = 100m;
            var quantity = 5;

            // Act
            var saleItem = new SaleItemBuilder()
                .WithUnitPrice(unitPrice)
                .WithQuantity(quantity)
                .Build();

            saleItem.TotalValue.ShouldBe(450m);
        }

        [Fact]
        public void CalculateTotal_With20PercentDiscount_ShouldApplyDiscountCorrectly()
        {
            // Arrange
            var unitPrice = 100m;
            var quantity = 10;

            // Act
            var saleItem = new SaleItemBuilder()
                .WithUnitPrice(unitPrice)
                .WithQuantity(quantity)
                .Build();

            saleItem.TotalValue.ShouldBe(800m);
        }
    }
}
