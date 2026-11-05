using Microsoft.EntityFrameworkCore;
using ParfumTree.Domain.Entities;

namespace ParfumTree.Infrastructure.Data;

public class ParfumTreeDbContext : DbContext
{
    public ParfumTreeDbContext(DbContextOptions<ParfumTreeDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<FragranceNote> FragranceNotes => Set<FragranceNote>();
    public DbSet<ProductNote> ProductNotes => Set<ProductNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductNote>()
            .HasOne(pn => pn.Product)
            .WithMany(p => p.ProductNotes)
            .HasForeignKey(pn => pn.ProductId);

        modelBuilder.Entity<ProductNote>()
            .HasOne(pn => pn.Note)
            .WithMany(n => n.ProductNotes)
            .HasForeignKey(pn => pn.NoteId);

        modelBuilder.Entity<ProductVariant>()
            .Property(pv => pv.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProductVariant>()
            .Property(pv => pv.OldPrice)
            .HasPrecision(18, 2);
    }
}