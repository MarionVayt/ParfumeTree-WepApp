using ParfumTree.Application.DTOs;

namespace ParfumTree.Application.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartByUserIdAsync(int userId);
    Task<CartDto> AddItemAsync(int userId, AddToCartDto dto);
    Task<CartDto> UpdateQuantityAsync(int userId, int productVariantId, int quantity);
    Task RemoveItemAsync(int userId, int productVariantId);
    Task ClearCartAsync(int userId);
}