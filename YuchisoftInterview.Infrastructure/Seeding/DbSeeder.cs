using Microsoft.EntityFrameworkCore;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Infrastructure.Persistence;
using static YuchisoftTest.Domain.Enums.Enums;

namespace YuchisoftTest.Infrastructure.Seeding
{
    public class DbSeeder
    {
        public static async Task MigrateAndSeedAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            var seeded = await context.SeedHistories.AnyAsync(x => x.Key == "InitialSeed");
            if (seeded) return;

            var kagit = new StockType { Name = "Kağıt", StockClass = StockClass.SarfMalzeme, IsActive = true };
            var mukavva = new StockType { Name = "Mukavva", StockClass = StockClass.SarfMalzeme, IsActive = true };

            context.StockTypes.AddRange(kagit, mukavva);
            await context.SaveChangesAsync(); 

            context.StockUnits.AddRange(
                new StockUnit
                {
                    StockTypeId = kagit.Id,
                    UnitCode = "KA-001",
                    QuantityUnit = QuantityUnit.Kilo,
                    Description = "Kağıt Kilo",
                    PurchasePrice = 100.12m,
                    PurchaseCurrency = Currency.USD,
                    SalePrice = null,
                    SaleCurrency = Currency.TRY,
                    IsActive = true
                },
                new StockUnit
                {
                    StockTypeId = mukavva.Id,
                    UnitCode = "MU-001",
                    QuantityUnit = QuantityUnit.Kilo,
                    Description = "Mukavva Kilo",
                    PurchasePrice = 20.00m,
                    PurchaseCurrency = Currency.TRY,
                    SalePrice = null,
                    SaleCurrency = Currency.TRY,
                    IsActive = true
                }
            );

            context.SeedHistories.Add(new SeedHistory { Key = "InitialSeed" });
            await context.SaveChangesAsync();
        }
    }
}
