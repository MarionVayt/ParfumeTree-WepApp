namespace ParfumTree.Application.DTOs;

public class ProductDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }

    public List<ProductVariantDto> Variants { get; set; } = new();

    public List<ProductNoteDto> Notes { get; set; } = new();
}