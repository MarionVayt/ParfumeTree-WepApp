namespace ParfumTree.Application.DTOs;

public class CreateProductVariantDto
{
    public int VolumeMl { get; set; }
    public string Concentration { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public int StockQuantity { get; set; }
    public string SKU { get; set; } = string.Empty;
}