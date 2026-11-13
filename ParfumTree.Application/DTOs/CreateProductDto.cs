namespace ParfumTree.Application.DTOs;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
}