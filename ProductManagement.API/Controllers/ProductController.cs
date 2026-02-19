using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Features.Product.DTOs;
using ProductManagement.Application.Features.Product.Interfaces;
using System.Security.Claims;

namespace ProductManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        var id = await _service.CreateAsync(dto, userId!);

        return Ok(id);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        await _service.UpdateAsync(dto, userId!);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
    [FromQuery] ProductQueryParameters parameters)
    {
        var result = await _service.GetPagedAsync(parameters);
        return Ok(result);
    }

    [HttpGet("category-summary")]
    public async Task<IActionResult> GetCategorySummary()
    {
        var result = await _service.GetProductsByCategoryAsync();
        return Ok(result);
    }


}
