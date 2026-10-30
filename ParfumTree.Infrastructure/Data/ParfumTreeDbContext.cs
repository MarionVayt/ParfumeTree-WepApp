using Microsoft.EntityFrameworkCore;
using ParfumTree.Domain.Entities;

namespace ParfumTree.Infrastructure.Data;

public class ParfumTreeDbContext : DbContext
{
    // 1. Конструктор прийняття конфігурацій (наприклад, Connection String до бази)
    public ParfumTreeDbContext(DbContextOptions<ParfumTreeDbContext> options) 
        : base(options)
    {
    }

    // 2. Таблиці бази даних (DbSet)
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<FragranceNote> FragranceNotes => Set<FragranceNote>();
    public DbSet<ProductNote> ProductNotes => Set<ProductNote>();

    // 3. Метод конфігурації зв'язків та обмежень (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Вказуємо EF Core, як влаштована проміжна таблиця піраміди нот (ProductNote)
        modelBuilder.Entity<ProductNote>()
            .HasOne(pn => pn.Product)
            .WithMany(p => p.ProductNotes)
            .HasForeignKey(pn => pn.ProductId);

        modelBuilder.Entity<ProductNote>()
            .HasOne(pn => pn.Note)
            .WithMany(n => n.ProductNotes)
            .HasForeignKey(pn => pn.NoteId);

        // Точність для грошових полів (decimal у PostgreSQL)
        modelBuilder.Entity<ProductVariant>()
            .Property(pv => pv.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProductVariant>()
            .Property(pv => pv.OldPrice)
            .HasPrecision(18, 2);
    }
}