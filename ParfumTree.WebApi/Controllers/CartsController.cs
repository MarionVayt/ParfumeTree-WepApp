using Microsoft.AspNetCore.Mvc;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;

namespace ParfumTree.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    
    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<CartDto>> GetCart(int userId)
    {
        var cart = await _cartService.GetCartByUserIdAsync(userId);
        return Ok(cart);
    }

    [HttpPost("user/{userId:int}/items")]
    public async Task<ActionResult<CartDto>> AddItem(int userId, AddToCartDto dto)
    {
        var cart = await _cartService.AddItemAsync(userId, dto);
        return Ok(cart);
    }

    [HttpPut("user/{userId:int}/items/{variantId:int}")]
    public async Task<ActionResult<CartDto>> UpdateItemQuantity(int userId, int variantId, [FromBody] AddToCartDto dto)
    {
        var updatedCart = await _cartService.UpdateQuantityAsync(userId, variantId, dto.Quantity);
        return Ok(updatedCart);
    }

    [HttpDelete("user/{userId:int}/items/{variantId:int}")]
    public async Task<IActionResult> RemoveItem(int userId, int variantId)
    {
        await _cartService.RemoveItemAsync(userId, variantId);
        return NoContent();
    }

    [HttpDelete("user/{userId:int}")]
    public async Task<IActionResult> ClearCart(int userId)
    {
        await _cartService.ClearCartAsync(userId);
        return NoContent();
    }
}