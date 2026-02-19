using ProductManagement.Domain.Common;

namespace ProductManagement.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public ICollection<ProductImage>? Images { get; set; }
}
