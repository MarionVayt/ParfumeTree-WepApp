using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;

namespace ParfumTree.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductListDto>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDetailDto>> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(new  { message = "Product not found." });
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDetailDto>> Create([FromBody] CreateProductDto dto)
    {
        var result = await _productService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        await _productService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }

}