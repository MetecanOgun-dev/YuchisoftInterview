using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Infrastructure.Seeding;

namespace YuchisoftTest.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<StockType> StockTypes => Set<StockType>();
        public DbSet<StockUnit> StockUnits => Set<StockUnit>();
        public DbSet<StockList> StockList => Set<StockList>();
        public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();
        public DbSet<SeedHistory> SeedHistories => Set<SeedHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockUnit>()
                .HasOne(x => x.StockType)
                .WithMany() 
                .HasForeignKey(x => x.StockTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockUnit>()
                .HasOne(x => x.StockList)
                .WithOne(x => x.StockUnit)
                .HasForeignKey<StockList>(x => x.StockUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockList>()
                .HasIndex(x => x.StockUnitId)
                .IsUnique();

            modelBuilder.Entity<SeedHistory>()
                .HasIndex(x => x.Key)
                .IsUnique();
        }
    }
}
