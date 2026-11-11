using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;
using ParfumTree.Domain.Entities;
using ParfumTree.Domain.Exceptions;
using ParfumTree.Infrastructure.Data;

namespace ParfumTree.Application.Services;

public class ProductService : IProductService
{
    private readonly ParfumTreeDbContext _context;

    public ProductService(ParfumTreeDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<ProductListDto>> GetProductsAsync(ProductQueryParameters parameters)
{
    var query = _context.Products
        .AsNoTracking()
        .AsQueryable();
    
    if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
    {
        var searchTerm = parameters.SearchTerm.ToLower();
        query = query.Where(p => 
            p.Name.ToLower().Contains(searchTerm) || 
            (p.Description != null && p.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.BrandId.HasValue)
        query = query.Where(p => p.BrandId == parameters.BrandId.Value);

    if (parameters.CategoryId.HasValue)
        query = query.Where(p => p.CategoryId == parameters.CategoryId.Value);

    if (!string.IsNullOrWhiteSpace(parameters.Gender))
        query = query.Where(p => p.Gender.ToLower() == parameters.Gender.ToLower());

    if (parameters.InStockOnly == true)
        query = query.Where(p => p.Variants.Any(v => v.StockQuantity > 0));
    
    if (parameters.MinPrice.HasValue)
        query = query.Where(p => p.Variants.Any() && p.Variants.Min(v => v.Price) >= parameters.MinPrice.Value);

    if (parameters.MaxPrice.HasValue)
        query = query.Where(p => p.Variants.Any() && p.Variants.Min(v => v.Price) <= parameters.MaxPrice.Value);
    
    var totalCount = await query.CountAsync();
    
    var items = await query
        .Skip((parameters.PageNumber - 1) * parameters.PageSize)
        .Take(parameters.PageSize)
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

    return new PagedResponse<ProductListDto>
    {
        Items = items,
        TotalCount = totalCount,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize
    };
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
        return createdProduct ?? throw new InvalidOperationException($"Не вдалося завантажити створений продукт з ID {product.Id} з бази даних.");
    }

    public async Task UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {id} was not found.");
        }
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Gender = dto.Gender;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
        {
            throw new NotFoundException($"Product with ID {id} was not found.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}