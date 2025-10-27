using _123Vendas.Domain.Sales.Unit.Builders;
using Shouldly;
using Xunit;

namespace _123Vendas.Domain.Sales.Unit.ValueObjects
{
    public class BranchTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateBranch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Filial 1";

            // Act
            var branch = new BranchBuilder()
                .WithId(id)
                .WithName(name)
                .Build();

            // Assert
            branch.ShouldNotBeNull();
            branch.Id.ShouldBe(id);
            branch.Name.ShouldBe(name);
        }

        [Fact]
        public void Constructor_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
                new BranchBuilder()
                    .WithEmptyId()
                    .Build())
                .Message.ShouldContain("ID da filial não pode ser vazio");
        }

        [Fact]
        public void Constructor_WithNullName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
                new BranchBuilder()
                    .WithNullName()
                    .Build())
                .Message.ShouldContain("Nome da filial não pode ser vazio");
        }

        [Fact]
        public void Constructor_WithWhitespaceName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
                new BranchBuilder()
                    .WithWhitespaceName()
                    .Build())
                .Message.ShouldContain("Nome da filial não pode ser vazio");
        }

        [Fact]
        public void Constructor_ShouldTrimName()
        {
            // Arrange
            var id = Guid.NewGuid();
            var nameWithSpaces = "  Filial 1  ";

            // Act
            var branch = new BranchBuilder()
                .WithId(id)
                .WithName(nameWithSpaces)
                .Build();

            // Assert
            branch.Name.ShouldBe("Filial 1");
        }
    }
}
