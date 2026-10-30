using ParfumTree.Domain.Common;
using ParfumTree.Domain.Entities;

namespace ParfumTree.Domain.Entities;

public class Product : BaseEntity
{
    public Brand Brand { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductNote> ProductNotes { get; set; } = new List<ProductNote>();
    public string Name { get; set; } =  string.Empty;
    public string? Description { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
}