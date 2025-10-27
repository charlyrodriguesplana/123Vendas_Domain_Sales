using AutoMapper;
using _123Vendas.Domain.Sales.Api.Common;
using _123Vendas.Domain.Sales.Api.Features.Sales.CreateSale;
using _123Vendas.Domain.Sales.Api.Features.Sales.CancelSale;
using _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create;
using _123Vendas.Domain.Sales.Application.UsesCase.Sale.Cancel;
using _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetAll;
using _123Vendas.Domain.Sales.Application.UsesCase.Sale.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _123Vendas.Domain.Sales.Api.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController(IMediator mediator, IMapper mapper) : BaseController
    {

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<SaleDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSales(CancellationToken cancellationToken)
        {
            var query = new GetAllSalesQuery();
            var sales = await mediator.Send(query, cancellationToken);

            return Ok(new ApiResponseWithData<List<SaleDto>>
            {
                Success = true,
                Data = sales
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<SaleDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSaleById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetSaleByIdQuery(id);
            var sale = await mediator.Send(query, cancellationToken);

            return Ok(new ApiResponseWithData<SaleDetailDto>
            {
                Success = true,
                Data = sale
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var command = mapper.Map<CreateSaleCommand>(request);
            var saleId = await mediator.Send(command, cancellationToken);

            var response = new CreateSaleResponse
            {
                Id = saleId,
                SaleNumber = request.SaleNumber,
                SaleDate = DateTime.UtcNow,
                TotalValue = request.Items.Sum(i => i.UnitPrice * i.Quantity),
                ItemsCount = request.Items.Count
            };

            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Venda criada com sucesso",
                Data = response
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSale(Guid id, CancellationToken cancellationToken)
        {
            var command = new CancelSaleCommand(id);
            await mediator.Send(command, cancellationToken);

            var response = new CancelSaleResponse
            {
                SaleId = id,
                CanceledAt = DateTime.UtcNow
            };

            return Ok(new ApiResponseWithData<CancelSaleResponse>
            {
                Success = true,
                Message = "Venda cancelada com sucesso",
                Data = response
            });
        }
    }
}
