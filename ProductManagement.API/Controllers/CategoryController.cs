using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Features.Category.DTOs;
using ProductManagement.Application.Features.Product.DTOs;
using ProductManagement.Infrastructure.Persistence;

namespace ProductManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/category
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _context.Categories
            .Where(c => !c.IsDeleted)
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return Ok(categories);
    }

    // POST: api/category
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = new Domain.Entities.Category
        {
            Name = dto.Name,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return Ok(category.Id);
    }

}
