using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;
using ParfumTree.WebApi.Extensions;

namespace ParfumTree.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart()
    {
        var cart = await _cartService.GetCartByUserIdAsync(User.GetUserId());
        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartDto>> AddItem([FromBody] AddToCartDto dto)
    {
        var cart = await _cartService.AddItemAsync(User.GetUserId(), dto);
        return Ok(cart);
    }

    [HttpPut("items/{variantId:int}")]
    public async Task<ActionResult<CartDto>> UpdateItemQuantity(int variantId, [FromBody] AddToCartDto dto)
    {
        var updatedCart = await _cartService.UpdateQuantityAsync(User.GetUserId(), variantId, dto.Quantity);
        return Ok(updatedCart);
    }

    [HttpDelete("items/{variantId:int}")]
    public async Task<IActionResult> RemoveItem(int variantId)
    {
        await _cartService.RemoveItemAsync(User.GetUserId(), variantId);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync(User.GetUserId());
        return NoContent();
    }
}