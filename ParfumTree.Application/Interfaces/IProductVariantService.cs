using ParfumTree.Application.DTOs;

namespace ParfumTree.Application.Interfaces;

public interface IProductVariantService
{
    Task<IEnumerable<ProductVariantDto>> GetVariantsByProductIdAsync(int productId);

    Task<ProductVariantDto?> CreateAsync(int productId, CreateProductVariantDto dto);
    Task<bool> UpdateAsync(int variantId, UpdateProductVariantDto dto);
    Task<bool> DeleteAsync(int variantId);
}