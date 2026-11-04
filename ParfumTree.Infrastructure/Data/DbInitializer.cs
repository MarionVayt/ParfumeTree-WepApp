using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParfumTree.Domain.Entities;
using ParfumTree.Domain.Enums;

namespace ParfumTree.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ParfumTreeDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var nicheCategory = new Category
        {
            Name = "Нішева парфумерія",
            Description = "Ексклюзивні аромати для поціновувачів"
        };
        var luxuryCategory = new Category 
        { 
            Name = "Люкс", 
            Description = "Популярні преміальні аромати від відомих будинків моди" 
        };

        await context.Categories.AddRangeAsync(nicheCategory, luxuryCategory);
        
        var tomFord = new Brand 
        { 
            Name = "Tom Ford", 
            Description = "Американський бренд розкоші та вишуканих ароматів", 
            LogoUrl = "https://example.com/tomford.jpg" 
        };
        var mfk = new Brand 
        { 
            Name = "Maison Francis Kurkdjian", 
            Description = "Французький нішевий парфумерний дім", 
            LogoUrl = "https://example.com/mfk.jpg" 
        };

        await context.Brands.AddRangeAsync(tomFord, mfk);

        // 4. Створюємо Ноти
        var noteTobacco = new FragranceNote { Name = "Листя тютюну" };
        var noteVanilla = new FragranceNote { Name = "Ваніль" };
        var noteWood = new FragranceNote { Name = "Деревні ноти" };
        var noteSaffron = new FragranceNote { Name = "Шафран" };
        var noteAmber = new FragranceNote { Name = "Сіра амбра" };

        await context.FragranceNotes.AddRangeAsync(noteTobacco, noteVanilla, noteWood, noteSaffron, noteAmber);

        // Зберігаємо первинні довідники, щоб EF Core згенерував для них Id
        await context.SaveChangesAsync();

        // 5. Створюємо Товар 1: Tom Ford Tobacco Vanille
        var tobaccoVanille = new Product
        {
            Name = "Tobacco Vanille",
            Description = "Теплий, пряний та східний аромат з багатим поєднанням тютюну та солодкої ванілі.",
            Gender = "Unisex",
            ReleaseYear = 2007,
            BrandId = tomFord.Id,
            CategoryId = nicheCategory.Id
        };

        // 6. Створюємо Товар 2: Baccarat Rouge 540
        var baccarat = new Product
        {
            Name = "Baccarat Rouge 540",
            Description = "Витончений та насичений аромат з нотами шафрану, ялини та сірої амбри.",
            Gender = "Unisex",
            ReleaseYear = 2015,
            BrandId = mfk.Id,
            CategoryId = nicheCategory.Id
        };

        await context.Products.AddRangeAsync(tobaccoVanille, baccarat);
        await context.SaveChangesAsync();

        // 7. Додаємо Піраміду нот для Tobacco Vanille
        var tobaccoNotes = new List<ProductNote>
        {
            new() { ProductId = tobaccoVanille.Id, NoteId = noteTobacco.Id, NoteType = NoteType.Top },
            new() { ProductId = tobaccoVanille.Id, NoteId = noteVanilla.Id, NoteType = NoteType.Heart },
            new() { ProductId = tobaccoVanille.Id, NoteId = noteWood.Id, NoteType = NoteType.Base }
        };

        // 8. Додаємо Варіанти об'ємів (ProductVariants)
        var tobaccoVariants = new List<ProductVariant>
        {
            new()
            {
                ProductId = tobaccoVanille.Id,
                VolumeMl = 50,
                ConcentrationType = ConcentrationType.EauDeParfum,
                Price = 4800.00m,
                OldPrice = 5200.00m,
                StockQuantity = 12,
                SKU = "TF-TV-50EDP",
                IsTester = false
            },
            new()
            {
                ProductId = tobaccoVanille.Id,
                VolumeMl = 100,
                ConcentrationType = ConcentrationType.EauDeParfum,
                Price = 7500.00m,
                StockQuantity = 5,
                SKU = "TF-TV-100EDP",
                IsTester = false
            }
        };

        await context.ProductNotes.AddRangeAsync(tobaccoNotes);
        await context.ProductVariants.AddRangeAsync(tobaccoVariants);

        // Фінальне збереження всіх зв'язків
        await context.SaveChangesAsync();
    }
}