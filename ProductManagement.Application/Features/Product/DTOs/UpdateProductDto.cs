namespace ProductManagement.Application.Features.Product.DTOs;

public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public bool IsActive { get; set; }
}
