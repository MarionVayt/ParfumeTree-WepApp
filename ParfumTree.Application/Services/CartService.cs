using Microsoft.EntityFrameworkCore;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;
using ParfumTree.Domain.Entities;
using ParfumTree.Domain.Exceptions;
using ParfumTree.Infrastructure.Data;

namespace ParfumTree.Application.Services;

public class CartService : ICartService
{
    private readonly ParfumTreeDbContext _context;

    public CartService(ParfumTreeDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> GetCartByUserIdAsync(int userId)
    {
        var cart = await _context.Carts
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(y => new CartDto
            {
                Id = y.Id,
                UserId = y.UserId,
                Items = y.CartItems.Select(yx => new CartItemDto
                {
                    Id = yx.Id,
                    ProductVariantId = yx.ProductVariantId,
                    ProductName = yx.ProductVariant.Product.Name,
                    BrandName = yx.ProductVariant.Product.Brand.Name,
                    VolumeMl = yx.ProductVariant.VolumeMl,
                    Concentration = yx.ProductVariant.Concentration.ToString(),
                    Price = yx.ProductVariant.Price,
                    Quantity = yx.Quantity
                }).ToList()
            })
            .FirstOrDefaultAsync();
        
        return cart ??  new CartDto
        {
            UserId = userId,
            Items = new List<CartItemDto>()
        };
    }

    public Task<CartDto> AddItemAsync(int userId, AddToCartDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<CartDto> AddItemToCartAsync(int userId, AddToCartDto dto)
    {
        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
        }
        
        var existingItem = cart.CartItems
            .FirstOrDefault(x => x.ProductVariantId == dto.ProductVariantId);

        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                ProductVariantId = dto.ProductVariantId,
                Quantity = dto.Quantity
            });
        }
        
        await _context.SaveChangesAsync();
        return await GetCartByUserIdAsync(userId);
    }

    public async Task<CartDto> UpdateQuantityAsync(int userId, int productVariantId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Кількість товару повинна бути більшою за 0.");
        }
        
        var cart = await  _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        
        if (cart == null)
        {
            throw new NotFoundException($"Кошик для користувача з ID {userId} не знайдено.");
        }
        
        var existingItem = cart.CartItems
            .FirstOrDefault(x => x.ProductVariantId == productVariantId);
        
        if (existingItem == null)
        {
            throw new NotFoundException($"Товар з ID {productVariantId} відсутній у кошику.");
        }
        
        existingItem.Quantity = quantity;
        
        await _context.SaveChangesAsync();
        return await GetCartByUserIdAsync(userId);
    }

    public async Task RemoveItemAsync(int userId, int productVariantId)
    {
        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
        {
            throw new NotFoundException($"Кошик для користувача з ID {userId} не знайдено.");
        }
    
        var existingItem = cart.CartItems
            .FirstOrDefault(x => x.ProductVariantId == productVariantId);
    
        if (existingItem == null)
        {
            throw new NotFoundException($"Товар з ID {productVariantId} відсутній у кошику.");
        }

        _context.CartItems.Remove(existingItem); 
        await _context.SaveChangesAsync();
    }

    public async Task ClearCartAsync(int userId)
    {
        var cart = await  _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
        {
            throw new NotFoundException($"Кошик для користувача з ID {userId} не знайдено.");
        }
        
        _context.CartItems.RemoveRange(cart.CartItems);
        await _context.SaveChangesAsync();  
    }
}