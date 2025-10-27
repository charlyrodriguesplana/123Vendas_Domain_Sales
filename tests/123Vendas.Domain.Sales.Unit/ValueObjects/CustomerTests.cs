using _123Vendas.Domain.Sales.Unit.Builders;
using Shouldly;
using Xunit;

namespace _123Vendas.Domain.Sales.Unit.ValueObjects
{
    public class CustomerTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateCustomer()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "charly Rodrigues";

            // Act
            var customer = new CustomerBuilder()
                .WithId(id)
                .WithName(name)
                .Build();

            // Assert
            customer.ShouldNotBeNull();
            customer.Id.ShouldBe(id);
            customer.Name.ShouldBe(name);
        }

        [Fact]
        public void Constructor_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
                new CustomerBuilder()
                    .WithEmptyId()
                    .Build())
                .Message.ShouldContain("ID do cliente não pode ser vazio");
        }

        [Fact]
        public void Constructor_WithNullName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
                new CustomerBuilder()
                    .WithNullName()
                    .Build())
                .Message.ShouldContain("Nome do cliente não pode ser vazio");
        }

        [Fact]
        public void Constructor_WithWhitespaceName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
                new CustomerBuilder()
                    .WithWhitespaceName()
                    .Build())
                .Message.ShouldContain("Nome do cliente não pode ser vazio");
        }

        [Fact]
        public void Constructor_ShouldTrimName()
        {
            // Arrange
            var id = Guid.NewGuid();
            var nameWithSpaces = "  charly Rodrigues  ";

            // Act
            var customer = new CustomerBuilder()
                .WithId(id)
                .WithName(nameWithSpaces)
                .Build();

            // Assert
            customer.Name.ShouldBe("charly Rodrigues");
        }
    }
}
