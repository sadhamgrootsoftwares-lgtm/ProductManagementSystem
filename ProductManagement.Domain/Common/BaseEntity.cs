namespace ProductManagement.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;

    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
