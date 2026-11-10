using FluentValidation;
using ParfumTree.Application.DTOs;

namespace ParfumTree.Application.Validators;

public class UpdateProductVariantDtoValidator : AbstractValidator<UpdateProductVariantDto>
{
    private static readonly int[] AllowedVolumes = [2, 5, 10, 30, 50, 100, 200];
    private static readonly string[] AllowedConcentrations = ["Parfum", "EDP", "EDT", "EDC", "Extrait"];

    public UpdateProductVariantDtoValidator()
    {
        RuleFor(x => x.VolumeMl)
            .GreaterThan(0).WithMessage("Об'єм має бути більшим за 0.")
            .Must(v => AllowedVolumes.Contains(v))
            .WithMessage($"Некоректний об'єм. Дозволені значення: {string.Join(", ", AllowedVolumes)} мл.");

        RuleFor(x => x.Concentration)
            .NotEmpty().WithMessage("Концентрація є обов'язковою.")
            .Must(c => AllowedConcentrations.Contains(c))
            .WithMessage($"Некоректна концентрація. Дозволені значення: {string.Join(", ", AllowedConcentrations)}.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Ціна повинна бути більшою за 0.");

        RuleFor(x => x.OldPrice)
            .GreaterThan(x => x.Price)
            .When(x => x.OldPrice.HasValue)
            .WithMessage("Стара ціна має бути більшою за поточну ціну.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Кількість на складі не може бути від'ємною.");
    }
}