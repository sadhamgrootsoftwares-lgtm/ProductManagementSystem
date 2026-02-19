namespace ProductManagement.Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = default!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}
