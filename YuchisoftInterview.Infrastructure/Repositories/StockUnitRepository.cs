using Microsoft.EntityFrameworkCore;
using YuchisoftTest.Application.Interface.Repository;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Infrastructure.Persistence;

namespace YuchisoftTest.Infrastructure.Repositories
{
    public class StockUnitRepository : IStockUnitRepository
    {
        private readonly AppDbContext _context;

        public StockUnitRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<StockUnit>> GetAllAsync(bool includeInactive = false)
        {
            var query = _context.StockUnits.Include(x => x.StockType).AsNoTracking();

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            return query.OrderBy(x => x.StockTypeId).ToListAsync();
        }

        public Task<StockUnit?> GetByIdAsync(int id, bool includeInactive = false)
        {
            var query = _context.StockUnits.Include(x => x.StockType).AsNoTracking().Where(x => x.Id == id);

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            return query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(StockUnit item)
        {
            _context.StockUnits.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StockUnit item)
        {
            _context.StockUnits.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task SetActiveAsync(int id, bool isActive)
        {
            var item = await _context.StockUnits.FirstOrDefaultAsync(x => x.Id == id);
            if (item is null) return;

            item.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.StockUnits.FirstOrDefaultAsync(x => x.Id == id);
            if (item is null) return;

            _context.StockUnits.Remove(item);
            await _context.SaveChangesAsync();
        }

        public Task<StockUnit?> GetByUnitCodeAsync(string unitCode, bool includeInactive = false)
        {
            var query = _context.StockUnits.AsNoTracking().Where(x => x.UnitCode == unitCode);

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            return query.FirstOrDefaultAsync();
        }

        public Task<bool> ExistsByUnitCodeAsync(string unitCode, int? excludeId = null, bool includeInactive = false)
        {
            var query = _context.StockUnits.Where(x => x.UnitCode.ToLower() == unitCode.ToLower());

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return query.AnyAsync();
        }
    }
}
