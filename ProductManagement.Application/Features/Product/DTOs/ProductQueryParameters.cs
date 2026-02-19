namespace ProductManagement.Application.Features.Product.DTOs;

public class ProductQueryParameters
{
    private const int MaxPageSize = 50;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? SearchTerm { get; set; }

    public int? CategoryId { get; set; }

    public bool? IsActive { get; set; }

    public string? SortBy { get; set; }

    public bool Descending { get; set; } = false;
}


