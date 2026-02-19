using ProductManagement.Application.Features.Category.DTOs;
using System.Net.Http.Json;

namespace ProductManagement.BlazorUI.Services;

public class CategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CategoryResponseDto>?> GetAllAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<CategoryResponseDto>>("api/category");
    }
}
