using ParfumTree.Domain.Common;
using ParfumTree.Domain.Enums;

namespace ParfumTree.Domain.Entities;

public class ProductNote : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int NoteId { get; set; }
    public FragranceNote Note { get; set; } = null!;
    public NoteType NoteType { get; set; }
}