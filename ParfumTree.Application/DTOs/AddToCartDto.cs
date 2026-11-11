namespace ParfumTree.Application.DTOs;

public class AddToCartDto
{
    public int ProductVariantId { get; set; }
    public int Quantity { get; set; } = 1;
}