using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Common.Models;
using ProductManagement.Application.Features.Product.DTOs;
using ProductManagement.Application.Features.Product.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Persistence;

namespace ProductManagement.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(CreateProductDto dto, string userId)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            IsActive = dto.IsActive,
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task UpdateAsync(UpdateProductDto dto, string userId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == dto.Id);

        if (product == null)
            throw new Exception("Product not found.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;
        product.IsActive = dto.IsActive;
        product.ModifiedBy = userId;
        product.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new Exception("Product not found.");

        product.IsDeleted = true;

        await _context.SaveChangesAsync();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive,
                CategoryName = p.Category.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive,
                CategoryName = p.Category.Name
            })
            .ToListAsync();
    }

    public async Task<PagedResult<ProductResponseDto>>
    GetPagedAsync(ProductQueryParameters parameters)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            query = query.Where(p =>
                p.Name.Contains(parameters.SearchTerm));
        }

        // Category Filter
        if (parameters.CategoryId.HasValue)
        {
            query = query.Where(p =>
                p.CategoryId == parameters.CategoryId);
        }

        // Active Filter
        if (parameters.IsActive.HasValue)
        {
            query = query.Where(p =>
                p.IsActive == parameters.IsActive);
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            if (parameters.SortBy.ToLower() == "price")
            {
                query = parameters.Descending
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price);
            }
            else
            {
                query = parameters.Descending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name);
            }
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive,
                CategoryName = p.Category.Name
            })
            .ToListAsync();

        return new PagedResult<ProductResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public async Task<List<ProductCategorySummaryDto>>
    GetProductsByCategoryAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted)
            .GroupBy(p => p.Category.Name)
            .Select(g => new ProductCategorySummaryDto
            {
                CategoryName = g.Key,
                ProductCount = g.Count()
            })
            .ToListAsync();
    }


}
