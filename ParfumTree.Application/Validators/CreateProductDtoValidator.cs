using FluentValidation;
using ParfumTree.Application.DTOs;

namespace ParfumTree.Application.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    private static readonly string[] AllowedGenders = ["Male", "Female", "Unisex"];
    
    public  CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва парфуму є обов'язковим.")
            .MaximumLength(50);
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Опис парфуму є обов'язковим.")
            .MaximumLength(200);

        RuleFor(x => x.BrandId)
            .GreaterThan(0).WithMessage("Вкажіть коректний ID бренду.");
        
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Вкажіть коректний ID категорії.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Гендер парфуму є обов'язковим.")
            .Must(g => AllowedGenders.Contains(g))
            .WithMessage("Некоректна стать. Дозволені значення: Male, Female, Unisex.");
        
        RuleFor(x => x.ReleaseYear)
            .InclusiveBetween(1700, DateTime.UtcNow.Year + 1)
            .WithMessage($"Рік випуску має бути в межах від 1700 до {DateTime.UtcNow.Year + 1}.");
    }
}