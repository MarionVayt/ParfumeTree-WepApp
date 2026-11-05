using System.Collections.Generic;
using System.Threading.Tasks;
using ParfumTree.Application.DTOs;

namespace ParfumTree.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductListDto>> GetAllProductsAsync();
    Task<ProductDetailDto?> GetProductByIdAsync(int id);
    Task<ProductDetailDto> CreateAsync(CreateProductDto dto);
    Task<bool> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}