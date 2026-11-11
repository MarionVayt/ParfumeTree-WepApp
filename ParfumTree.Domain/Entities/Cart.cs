using ParfumTree.Domain.Common;

namespace ParfumTree.Domain.Entities;

public class Cart : BaseEntity
{
    public int UserId { get; set; }
    public ICollection<CartItem> CartItems { get; set; } =  new List<CartItem>();
}