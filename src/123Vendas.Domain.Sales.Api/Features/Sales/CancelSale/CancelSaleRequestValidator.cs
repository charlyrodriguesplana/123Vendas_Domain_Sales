using FluentValidation;

namespace _123Vendas.Domain.Sales.Api.Features.Sales.CancelSale
{
    public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
    {
        public CancelSaleRequestValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty()
                .WithMessage("ID da venda é obrigatório");
        }
    }
}
