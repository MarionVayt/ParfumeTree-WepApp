using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;
using ParfumTree.Domain.Entities;
using ParfumTree.Infrastructure.Data;

namespace ParfumTree.Application.Services;

public class ProductService : IProductService
{
    private readonly ParfumTreeDbContext _context;

    public ProductService(ParfumTreeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductListDto>> GetAllProductsAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                BrandName = p.Brand.Name,
                CategoryName = p.Category.Name,
                Gender = p.Gender,
                MinPrice = p.Variants.Any() ? p.Variants.Min(v => v.Price) : 0,
                IsInStock = p.Variants.Any(v => v.StockQuantity > 0)
            })
            .ToListAsync();
    }

    public async Task<ProductDetailDto?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDetailDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BrandName = p.Brand.Name,
                CategoryName = p.Category.Name,
                Gender = p.Gender,
                ReleaseYear = p.ReleaseYear,
                Variants = p.Variants.Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    VolumeMl = v.VolumeMl,
                    Concentration = v.Concentration.ToString(),
                    Price = v.Price,
                    OldPrice = v.OldPrice,
                    StockQuantity = v.StockQuantity,
                    SKU = v.SKU,
                    IsTester = v.IsTester
                }).ToList(),
                Notes = p.ProductNotes.Select(pn => new ProductNoteDto
                {
                    NoteId = pn.NoteId,
                    NoteName = pn.Note.Name,
                    NoteType = pn.NoteType.ToString()
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDetailDto> CreateAsync(CreateProductDto dto)
    {

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            BrandId = dto.BrandId,
            CategoryId = dto.CategoryId,
            Gender = dto.Gender
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        var createdProduct = await GetProductByIdAsync(product.Id);
        return createdProduct!;
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false; 
        }
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Gender = dto.Gender;
        
        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}