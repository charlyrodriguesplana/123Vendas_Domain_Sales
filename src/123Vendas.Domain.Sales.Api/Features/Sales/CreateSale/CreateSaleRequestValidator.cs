using FluentValidation;

namespace _123Vendas.Domain.Sales.Api.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .WithMessage("Número da venda é obrigatório")
                .MaximumLength(50)
                .WithMessage("Número da venda não pode exceder 50 caracteres");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("ID do cliente é obrigatório");

            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .WithMessage("Nome do cliente é obrigatório")
                .MaximumLength(200)
                .WithMessage("Nome do cliente não pode exceder 200 caracteres");

            RuleFor(x => x.BranchId)
                .NotEmpty()
                .WithMessage("ID da filial é obrigatório");

            RuleFor(x => x.BranchName)
                .NotEmpty()
                .WithMessage("Nome da filial é obrigatório")
                .MaximumLength(200)
                .WithMessage("Nome da filial não pode exceder 200 caracteres");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Pelo menos um item é obrigatório")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("A venda deve conter pelo menos um item");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateSaleItemRequestValidator());
        }
    }

    public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ID do produto é obrigatório");

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Nome do produto é obrigatório")
                .MaximumLength(200)
                .WithMessage("Nome do produto não pode exceder 200 caracteres");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Preço unitário deve ser maior que zero");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantidade deve ser maior que zero")
                .LessThanOrEqualTo(20)
                .WithMessage("Não é possível vender mais de 20 itens idênticos");
        }
    }
}
