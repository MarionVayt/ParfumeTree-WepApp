using ParfumTree.Domain.Common;

namespace ParfumTree.Domain.Entities;

public class FragranceNote : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<ProductNote> ProductNotes { get; set; } = new List<ProductNote>();
}