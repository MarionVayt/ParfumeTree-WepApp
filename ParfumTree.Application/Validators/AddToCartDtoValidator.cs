using FluentValidation;
using ParfumTree.Application.DTOs;

namespace ParfumTree.Application.Validators;

public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
{
    public AddToCartDtoValidator()
    {
        RuleFor(x => x.ProductVariantId)
            .GreaterThan(0).WithMessage("Вкажіть коректний ID варіанту товару.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Кількість товару повинна бути більшою за 0.");
    }
}