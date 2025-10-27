using _123Vendas.Domain.Sales.Domain.Entities;
using _123Vendas.Domain.Sales.Domain.Enums;
using _123Vendas.Domain.Sales.Domain.Events;
using _123Vendas.Domain.Sales.Domain.Exceptions;
using _123Vendas.Domain.Sales.Unit.Builders;
using Shouldly;
using Xunit;

namespace _123Vendas.Domain.Sales.Unit.Entities
{
    public class SaleTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateSale()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act
            var sale = Sale.Create(saleNumber, saleDate, customer, branch, items);

            // Assert
            sale.ShouldNotBeNull();
            sale.SaleNumber.ShouldBe(saleNumber);
            sale.SaleDate.ShouldBe(saleDate);
            sale.CustomerId.ShouldBe(customer.Id);
            sale.CustomerName.ShouldBe(customer.Name);
            sale.BranchId.ShouldBe(branch.Id);
            sale.BranchName.ShouldBe(branch.Name);
            sale.Items.Count().ShouldBe(1);
        }

        [Fact]
        public void Create_WithValidData_ShouldGenerateId()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act
            var sale = Sale.Create(saleNumber, saleDate, customer, branch, items);

            // Assert
            sale.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void Create_WithValidData_ShouldSetStatusAsActive()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act
            var sale = Sale.Create(saleNumber, saleDate, customer, branch, items);

            // Assert
            sale.Status.ShouldBe(SaleStatus.Active);
        }

        [Fact]
        public void Create_WithValidData_ShouldRegisterSaleCreatedEvent()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act
            var sale = Sale.Create(saleNumber, saleDate, customer, branch, items);

            // Assert
            sale.Events.Count().ShouldBe(1);
            var createdEvent = sale.Events.First() as SaleCreatedEvent;
            createdEvent.ShouldNotBeNull();
            createdEvent.SaleId.ShouldBe(sale.Id);
            createdEvent.SaleNumber.ShouldBe(saleNumber);
            createdEvent.TotalValue.ShouldBe(sale.TotalValue);
        }

        [Fact]
        public void Create_WithNullSaleNumber_ShouldThrowDomainException()
        {
            // Arrange
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act, Assert
            Should.Throw<DomainException>(() =>
                Sale.Create(null!, saleDate, customer, branch, items))
                .Message.ShouldBe("Número da venda é obrigatório");
        }

        [Fact]
        public void Create_WithEmptySaleNumber_ShouldThrowDomainException()
        {
            // Arrange
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act, Assert
            Should.Throw<DomainException>(() =>
                Sale.Create("", saleDate, customer, branch, items))
                .Message.ShouldBe("Número da venda é obrigatório");
        }

        [Fact]
        public void Create_WithWhitespaceSaleNumber_ShouldThrowDomainException()
        {
            // Arrange
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act, Assert
            Should.Throw<DomainException>(() =>
                Sale.Create("   ", saleDate, customer, branch, items))
                .Message.ShouldBe("Número da venda é obrigatório");
        }

        [Fact]
        public void Create_WithEmptyItems_ShouldThrowDomainException()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem>();

            // Act, Assert
            Should.Throw<DomainException>(() =>
                Sale.Create(saleNumber, saleDate, customer, branch, items))
                .Message.ShouldBe("A venda deve conter pelo menos um item");
        }

        [Fact]
        public void Create_ShouldCopyCustomerIdAndName()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = new CustomerBuilder()
                .WithId(Guid.NewGuid())
                .WithName("John Doe")
                .Build();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act
            var sale = Sale.Create(saleNumber, saleDate, customer, branch, items);

            // Assert
            sale.CustomerId.ShouldBe(customer.Id);
            sale.CustomerName.ShouldBe(customer.Name);
        }

        [Fact]
        public void Create_ShouldCopyBranchIdAndName()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = new BranchBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Main Branch")
                .Build();
            var items = new List<SaleItem> { SaleItemBuilder.BuildValid() };

            // Act
            var sale = Sale.Create(saleNumber, saleDate, customer, branch, items);

            // Assert
            sale.BranchId.ShouldBe(branch.Id);
            sale.BranchName.ShouldBe(branch.Name);
        }

        [Fact]
        public void TotalValue_ShouldSumAllItemsTotalValue()
        {
            // Arrange
            var saleNumber = "SALE-001";
            var saleDate = DateTime.UtcNow;
            var customer = CustomerBuilder.BuildValid();
            var branch = BranchBuilder.BuildValid();
            var items = new List<SaleItem>
            {
                new SaleItemBuilder().WithUnitPrice(100m).WithQuantity(2).Build(), // 200
                new SaleItemBuilder().WithUnitPrice(50m).WithQuantity(3).Build(),  // 150 (sem desconto)
                new SaleItemBuilder().WithUnitPrice(100m).WithQuantity(5).Build()  // 450 (10% desconto: 500 - 50)
            };

            // Act
            var sale = Sale.Create(saleNumber, saleDate, customer, branch, items);

            // Assert
            sale.TotalValue.ShouldBe(800m); // 200 + 150 + 450
        }

        [Fact]
        public void CancelSale_WhenActive_ShouldChangeStatusToCancelled()
        {
            // Arrange
            var sale = Sale.Create(
                "SALE-001",
                DateTime.UtcNow,
                CustomerBuilder.BuildValid(),
                BranchBuilder.BuildValid(),
                new List<SaleItem> { SaleItemBuilder.BuildValid() }
            );

            // Act
            sale.CancelSale();

            // Assert
            sale.Status.ShouldBe(SaleStatus.Cancelled);
        }

        [Fact]
        public void CancelSale_WhenActive_ShouldRegisterSaleCanceledEvent()
        {
            // Arrange
            var sale = Sale.Create(
                "SALE-001",
                DateTime.UtcNow,
                CustomerBuilder.BuildValid(),
                BranchBuilder.BuildValid(),
                new List<SaleItem> { SaleItemBuilder.BuildValid() }
            );

            sale.ClearEvents();

            // Act
            sale.CancelSale();

            // Assert
            sale.Events.Count().ShouldBe(1);
            var canceledEvent = sale.Events.First() as SaleCanceledEvent;
            canceledEvent.ShouldNotBeNull();
            canceledEvent.SaleId.ShouldBe(sale.Id);
            canceledEvent.SaleNumber.ShouldBe(sale.SaleNumber);
        }

        [Fact]
        public void CancelSale_WhenAlreadyCancelled_ShouldNotRegisterEventAgain()
        {
            // Arrange
            var sale = Sale.Create(
                "SALE-001",
                DateTime.UtcNow,
                CustomerBuilder.BuildValid(),
                BranchBuilder.BuildValid(),
                new List<SaleItem> { SaleItemBuilder.BuildValid() }
            );

            sale.CancelSale();
            sale.ClearEvents();

            // Act
            sale.CancelSale();

            // Assert
            sale.Events.Count().ShouldBe(0);
        }

        [Fact]
        public void CancelItem_WithExistingProductId_ShouldNotThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var item = new SaleItemBuilder().WithProductId(productId).Build();
            var sale = Sale.Create(
                "SALE-001",
                DateTime.UtcNow,
                CustomerBuilder.BuildValid(),
                BranchBuilder.BuildValid(),
                new List<SaleItem> { item }
            );

            // Act, Assert
            Should.NotThrow(() => sale.CancelItem(productId));
        }

        [Fact]
        public void CancelItem_WithNonExistingProductId_ShouldThrowDomainException()
        {
            // Arrange
            var sale = Sale.Create(
                "SALE-001",
                DateTime.UtcNow,
                CustomerBuilder.BuildValid(),
                BranchBuilder.BuildValid(),
                new List<SaleItem> { SaleItemBuilder.BuildValid() }
            );

            var nonExistingProductId = Guid.NewGuid();

            // Act, Assert
            Should.Throw<DomainException>(() =>
                sale.CancelItem(nonExistingProductId))
                .Message.ShouldBe("Item não encontrado para essa venda.");
        }

        [Fact]
        public void ClearEvents_ShouldRemoveAllEvents()
        {
            // Arrange
            var sale = Sale.Create(
                "SALE-001",
                DateTime.UtcNow,
                CustomerBuilder.BuildValid(),
                BranchBuilder.BuildValid(),
                new List<SaleItem> { SaleItemBuilder.BuildValid() }
            );

            sale.Events.Count().ShouldBeGreaterThan(0);

            // Act
            sale.ClearEvents();

            // Assert
            sale.Events.Count().ShouldBe(0);
        }
    }
}
