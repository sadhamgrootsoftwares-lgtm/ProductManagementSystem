using ProductManagement.Application.Common.Models;
using ProductManagement.Application.Features.Product.DTOs;

namespace ProductManagement.Application.Features.Product.Interfaces;

public interface IProductService
{
    Task<int> CreateAsync(CreateProductDto dto, string userId);
    Task UpdateAsync(UpdateProductDto dto, string userId);
    Task DeleteAsync(int id);
    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<List<ProductResponseDto>> GetAllAsync();

    Task<PagedResult<ProductResponseDto>>GetPagedAsync(ProductQueryParameters parameters);
    Task<List<ProductCategorySummaryDto>> GetProductsByCategoryAsync();

}
