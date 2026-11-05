using Microsoft.AspNetCore.Mvc;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;

namespace ParfumTree.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductVariantsController : ControllerBase
{
    private readonly IProductVariantService _variantService;
    
    public ProductVariantsController(IProductVariantService variantService)
    {
        _variantService = variantService;
    }

    [HttpGet("product/{productId:int}")]
    public async Task<ActionResult<IEnumerable<ProductVariantDto>>> GetByProductId(int productId)
    {
        var variants = await _variantService.GetVariantsByProductIdAsync(productId);
        return Ok(variants);
    }

    [HttpPost("product/{productId:int}")]
    public async Task<ActionResult<ProductVariantDto?>> Create(int productId, [FromBody] CreateProductVariantDto dto)
    {
        var result = await _variantService.CreateAsync(productId, dto);
        if (result == null)
        {
            return NotFound("Парфум з таким ID не знайдено");
        }
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductVariantDto dto)
    {
        var isUpdated = await _variantService.UpdateAsync(id, dto);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _variantService.DeleteAsync(id);
        if (!isDeleted) return NotFound();

        return NoContent();
    }
}