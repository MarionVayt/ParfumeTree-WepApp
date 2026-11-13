using Microsoft.EntityFrameworkCore;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;
using ParfumTree.Domain.Entities;
using ParfumTree.Domain.Enums;
using ParfumTree.Domain.Exceptions;
using ParfumTree.Infrastructure.Data;

namespace ParfumTree.Application.Services;

public class ProductVariantService : IProductVariantService
{
    private readonly ParfumTreeDbContext _context;

    public ProductVariantService(ParfumTreeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductVariantDto>> GetVariantsByProductIdAsync(int productId)
    {
        return await _context.ProductVariants
            .AsNoTracking()
            .Where(v => v.ProductId == productId)
            .Select(v => new ProductVariantDto
            {
                Id = v.Id,
                VolumeMl = v.VolumeMl,
                Concentration = v.Concentration.ToString(),
                Price = v.Price,
                OldPrice = v.OldPrice,
                StockQuantity = v.StockQuantity,
                SKU = v.SKU
            })
            .ToListAsync();
    }

    public async Task<ProductVariantDto?> CreateAsync(int productId, CreateProductVariantDto dto)
    {
        var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
        if (!productExists) return null;

        var concentration = dto.Concentration.ToUpper() switch
        {
            "EDP" => ConcentrationType.EauDeParfum,
            "EDT" => ConcentrationType.EauDeToilette,
            "EDC" => ConcentrationType.EauDeCologne,
            "PARFUM" => ConcentrationType.Parfum,
            "EXTRAIT" => ConcentrationType.ExtraitDeParfum,
            _ => throw new ArgumentException("Некоректна концентрація")
        };

        var variant = new ProductVariant
        {
            ProductId = productId,
            VolumeMl = dto.VolumeMl,
            Concentration = concentration,
            Price = dto.Price,
            OldPrice = dto.OldPrice,
            StockQuantity = dto.StockQuantity,
            SKU = dto.SKU
        };

        _context.ProductVariants.Add(variant);
        await _context.SaveChangesAsync();

        return new ProductVariantDto
        {
            Id = variant.Id,
            VolumeMl = variant.VolumeMl,
            Concentration = variant.Concentration.ToString(),
            Price = variant.Price,
            OldPrice = variant.OldPrice,
            StockQuantity = variant.StockQuantity,
            SKU = dto.SKU
        };
    }

    public async Task UpdateAsync(int variantId, UpdateProductVariantDto dto)
    {
        var variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.Id == variantId);
        if (variant == null)
        {
            throw new NotFoundException($"Product with ID {variantId} was not found.");
        }

        var concentration = dto.Concentration.ToUpper() switch
        {
            "EDP" => ConcentrationType.EauDeParfum,
            "EDT" => ConcentrationType.EauDeToilette,
            "EDC" => ConcentrationType.EauDeCologne,
            "PARFUM" => ConcentrationType.Parfum,
            "EXTRAIT" => ConcentrationType.ExtraitDeParfum,
            _ => throw new ArgumentException("Некоректна концентрація")
        };

        variant.VolumeMl = dto.VolumeMl;
        variant.Concentration = concentration;
        variant.Price = dto.Price;
        variant.OldPrice = dto.OldPrice;
        variant.StockQuantity = dto.StockQuantity;
        variant.SKU = dto.SKU;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int variantId)
    {
        var variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.Id == variantId);
        if (variant == null)
        {
            throw new NotFoundException($"Product variant with ID {variantId} was not found.");
        }

        _context.ProductVariants.Remove(variant);
        await _context.SaveChangesAsync();

    }
}