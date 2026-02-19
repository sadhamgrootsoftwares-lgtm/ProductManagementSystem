using ProductManagement.Application.Common.Models;
using ProductManagement.Application.Features.Product.DTOs;
using System.Net.Http.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductManagement.BlazorUI.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;
    private readonly ToastService _toastService;

    public ProductService(HttpClient httpClient,
                          ToastService toastService)
    {
        _httpClient = httpClient;
        _toastService = toastService;
    }
    public async Task<List<ProductResponseDto>?> GetAllAsync()
    {

        try
        {
            return await _httpClient
            .GetFromJsonAsync<List<ProductResponseDto>>("api/product");
        }
        catch
        {
            _toastService.Show("Something went wrong", false);
            return null;
        }
        
    }

    public async Task<int?> CreateAsync(CreateProductDto dto)
    {
        var response = await _httpClient
            .PostAsJsonAsync("api/product", dto);

        if (!response.IsSuccessStatusCode)
            return null;
        try
        {
            return await response.Content.ReadFromJsonAsync<int>();
        }
        catch
        {
            _toastService.Show("Something went wrong", false);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient
            .DeleteAsync($"api/product/{id}");

        try
        {
            return response.IsSuccessStatusCode;
        }
        catch
        {
            _toastService.Show("Something went wrong", false);
            return false;
        }

    }

    public async Task<UpdateProductDto?> GetByIdAsync(int id)
    {
        try
        {
        return await _httpClient
            .GetFromJsonAsync<UpdateProductDto>($"api/product/{id}");
           
        }
        catch
        {
            _toastService.Show("Something went wrong", false);
            return null;
        }
    }

    public async Task<bool> UpdateAsync(UpdateProductDto dto)
    {
        var response = await _httpClient
            .PutAsJsonAsync($"api/product/{dto.Id}", dto);

        try
        {
        return response.IsSuccessStatusCode;
        }
        catch
        {
            _toastService.Show("Something went wrong", false);
            return false;
        }
    }
    public async Task<PagedResult<ProductResponseDto>?>
    GetPagedAsync(ProductQueryParameters parameters)
    {
        var query = $"api/product/paged?" +
                    $"PageNumber={parameters.PageNumber}&" +
                    $"PageSize={parameters.PageSize}";

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query += $"&SearchTerm={parameters.SearchTerm}";

        if (parameters.CategoryId.HasValue)
            query += $"&CategoryId={parameters.CategoryId.Value}";

        if (parameters.IsActive.HasValue)
            query += $"&IsActive={parameters.IsActive.Value}";

        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            query += $"&SortBy={parameters.SortBy}";

        if (parameters.Descending)
            query += $"&Descending=true";
        
        try
        {
            return await _httpClient
            .GetFromJsonAsync<PagedResult<ProductResponseDto>>(query);

        }
        catch
        {
            _toastService.Show("Something went wrong", false);
            return null;
        }
    }

    public async Task<List<ProductCategorySummaryDto>?>
    GetCategorySummaryAsync()
    {
        try
        {
            return await _httpClient
           .GetFromJsonAsync<List<ProductCategorySummaryDto>>
           ("api/product/category-summary");

        }
        catch
        {
            _toastService.Show("Something went wrong", false);
            return null;
        }
    }

}
