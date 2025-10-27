using _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create;
using _123Vendas.Domain.Sales.Application.UsesCase.Shared;
using _123Vendas.Domain.Sales.Domain.Entities;
using _123Vendas.Domain.Sales.Domain.Events;
using _123Vendas.Domain.Sales.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace _123Vendas.Domain.Sales.Unit.UseCases
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateSaleCommandHandler> _logger;
        private readonly CreateSaleCommandHandler _handler;

        public CreateSaleCommandHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mediator = Substitute.For<IMediator>();
            _logger = Substitute.For<ILogger<CreateSaleCommandHandler>>();
            _handler = new CreateSaleCommandHandler(_saleRepository, _mediator, _logger);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateSale()
        {
            // Arrange
            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: Guid.NewGuid(),
                CustomerName: "Charly R",
                BranchId: Guid.NewGuid(),
                BranchName: "FIlial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2
                    )
                }
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCallRepositoryAdd()
        {
            // Arrange
            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: Guid.NewGuid(),
                CustomerName: "Charly R",
                BranchId: Guid.NewGuid(),
                BranchName: "Filial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2
                    )
                }
            );

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).AddAsync(
                Arg.Any<Sale>(),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCallRepositorySaveChanges()
        {
            // Arrange
            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: Guid.NewGuid(),
                CustomerName: "Charly R",
                BranchId: Guid.NewGuid(),
                BranchName: "Filial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2
                    )
                }
            );

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldPublishSaleCreatedEvent()
        {
            // Arrange
            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: Guid.NewGuid(),
                CustomerName: "Charly R",
                BranchId: Guid.NewGuid(),
                BranchName: "Filial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2
                    )
                }
            );

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _mediator.Received(1).Publish(
                Arg.Any<SaleCreatedEvent>(),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldReturnSaleId()
        {
            // Arrange
            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: Guid.NewGuid(),
                CustomerName: "Charly R",
                BranchId: Guid.NewGuid(),
                BranchName: "Filial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2
                    )
                }
            );

            // Act
            var saleId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            saleId.ShouldNotBe(Guid.Empty);
            saleId.ShouldBeOfType<Guid>();
        }

        [Fact]
        public async Task Handle_WithMultipleItems_ShouldCalculateTotalValueCorrectly()
        {
            // Arrange
            Sale? capturedSale = null;
            await _saleRepository.AddAsync(Arg.Do<Sale>(s => capturedSale = s), Arg.Any<CancellationToken>());

            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: Guid.NewGuid(),
                CustomerName: "Charly R",
                BranchId: Guid.NewGuid(),
                BranchName: "Filial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2 
                    ),
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 2",
                        UnitPrice: 50m,
                        Quantity: 3 
                    ),
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 3",
                        UnitPrice: 100m,
                        Quantity: 5 
                    )
                }
            );

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedSale.ShouldNotBeNull();
            capturedSale.TotalValue.ShouldBe(800m);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCallAddBeforeSaveChanges()
        {
            // Arrange
            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: Guid.NewGuid(),
                CustomerName: "Charly R",
                BranchId: Guid.NewGuid(),
                BranchName: "Filial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: Guid.NewGuid(),
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2
                    )
                }
            );

            var callOrder = new List<string>();

            _saleRepository.When(x => x.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()))
                .Do(_ => callOrder.Add("AddAsync"));

            _saleRepository.When(x => x.SaveChangesAsync(Arg.Any<CancellationToken>()))
                .Do(_ => callOrder.Add("SaveChangesAsync"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            callOrder.Count.ShouldBe(2);
            callOrder[0].ShouldBe("AddAsync");
            callOrder[1].ShouldBe("SaveChangesAsync");
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldMapCommandDataToSale()
        {
            // Arrange
            Sale? capturedSale = null;
            await _saleRepository.AddAsync(Arg.Do<Sale>(s => capturedSale = s), Arg.Any<CancellationToken>());

            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var ProductId = Guid.NewGuid();

            var command = new CreateSaleCommand(
                SaleNumber: "SALE-001",
                CustomerId: customerId,
                CustomerName: "Charly R",
                BranchId: branchId,
                BranchName: "Filial 1",
                Items: new List<SaleItemDto>
                {
                    new SaleItemDto(
                        ProductId: ProductId,
                        ProductName: "Produto 1",
                        UnitPrice: 100m,
                        Quantity: 2
                    )
                }
            );

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedSale.ShouldNotBeNull();
            capturedSale.SaleNumber.ShouldBe("SALE-001");
            capturedSale.CustomerId.ShouldBe(customerId);
            capturedSale.CustomerName.ShouldBe("Charly R");
            capturedSale.BranchId.ShouldBe(branchId);
            capturedSale.BranchName.ShouldBe("Filial 1");
            capturedSale.Items.Count().ShouldBe(1);
            capturedSale.Items.First().ProductId.ShouldBe(ProductId);
            capturedSale.Items.First().ProductName.ShouldBe("Produto 1");
        }
    }
}
