using ParfumTree.Domain.Common;

namespace ParfumTree.Domain.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl  { get; set; }
}