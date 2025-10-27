using AutoMapper;
using _123Vendas.Domain.Sales.Api.Features.Sales.CreateSale;
using _123Vendas.Domain.Sales.Application.UsesCase.Sale.Create;
using _123Vendas.Domain.Sales.Application.UsesCase.Shared;

namespace _123Vendas.Domain.Sales.Api.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, SaleItemDto>();
        }
    }
}
