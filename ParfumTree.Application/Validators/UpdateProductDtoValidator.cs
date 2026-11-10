using FluentValidation;
using ParfumTree.Application.DTOs;

namespace ParfumTree.Application.Validators;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    private static readonly string[] AllowedGenders = ["Male", "Female", "Unisex"];
    
    public  UpdateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва парфуму є обов'язковим.")
            .MaximumLength(50);
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Опис парфуму є обов'язковим.")
            .MaximumLength(200);

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Гендер парфуму є обов'язковим.")
            .Must(g => AllowedGenders.Contains(g))
            .WithMessage("Некоректна стать. Дозволені значення: Male, Female, Unisex.");
    }
}