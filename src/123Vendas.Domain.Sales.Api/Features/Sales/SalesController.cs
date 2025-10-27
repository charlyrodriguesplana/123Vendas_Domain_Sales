using AutoMapper;
using _123Vendas.Domain.Sales.Api.Common;
using _123Vendas.Domain.Sales.Api.Features.Sales.CreateSale;
using _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _123Vendas.Domain.Sales.Api.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController(IMediator mediator, IMapper mapper) : BaseController
    {
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
    }
}
