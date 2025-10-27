using AutoMapper;
using _123Vendas.Domain.Sales.Api.Features.Sales.CreateSale;
using _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create;
using _123Vendas.Domain.Sales.Application.UsesCase.Shared;

namespace _123Vendas.Domain.Sales.Api.Mappings
{
    /// <summary>
    /// AutoMapper profile for Sale mappings
    /// </summary>
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            // Request to Command mappings
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, SaleItemDto>();
        }
    }
}
