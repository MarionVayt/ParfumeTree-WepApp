using ParfumTree.Domain.Common;
using ParfumTree.Domain.Enums;

namespace ParfumTree.Domain.Entities;

public class ProductVariant : BaseEntity
{
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    public int VolumeMl { get; set; }
    public ConcentrationType Concentration { get; set; }
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public int StockQuantity { get; set; }
    public string SKU { get; set; } = string.Empty;
}