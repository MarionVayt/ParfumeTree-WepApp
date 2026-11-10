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
                StockQuantity = v.StockQuantity
            })
            .ToListAsync();
    }

    public async Task<ProductVariantDto?> CreateAsync(int productId, CreateProductVariantDto dto)
    {
        var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
        if (!productExists) return null;

        Enum.TryParse<ConcentrationType>(dto.Concentration, true, out var concentration);

        var variant = new ProductVariant
        {
            ProductId = productId,
            VolumeMl = dto.VolumeMl,
            Concentration = concentration,
            Price = dto.Price,
            OldPrice = dto.OldPrice,
            StockQuantity = dto.StockQuantity
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
            StockQuantity = variant.StockQuantity
        };
    }

    public async Task UpdateAsync(int variantId, UpdateProductVariantDto dto)
    {
        var variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.Id == variantId);
        if (variant == null)
        {
            throw new NotFoundException($"Product with ID {variantId} was not found.");
        }

        Enum.TryParse<ConcentrationType>(dto.Concentration, true, out var concentration);

        variant.VolumeMl = dto.VolumeMl;
        variant.Concentration = concentration;
        variant.Price = dto.Price;
        variant.OldPrice = dto.OldPrice;
        variant.StockQuantity = dto.StockQuantity;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int variantId)
    {
        var variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.Id == variantId);
        if (variant == null)
        {
            throw new NotFoundException($"Product with ID {variantId} was not found.");
        }

        _context.ProductVariants.Remove(variant);
        await _context.SaveChangesAsync();

    }
}