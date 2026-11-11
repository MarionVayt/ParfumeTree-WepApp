using ParfumTree.Domain.Entities;

namespace ParfumTree.Application.DTOs;

public class CartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    
    public decimal GrandTotal => Items.Sum(x => x.TotalPrice);
}