using Microsoft.EntityFrameworkCore;
using YuchisoftTest.Application.Interface.Repository;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Infrastructure.Persistence;

namespace YuchisoftTest.Infrastructure.Repositories
{
    public class StockListRepository : IStockListRepository
    {
        private readonly AppDbContext _context;

        public StockListRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<StockList>> GetAllAsync(bool includeInactive = false)
        {
            var query = _context.StockList
                .AsNoTracking()
                .Include(x => x.StockUnit)
                .ThenInclude(u => u.StockType)
                .AsQueryable();

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            return query
                .OrderBy(x => x.StockUnitId)                
                .ThenBy(x => x.StockUnit.UnitCode)           
                .ToListAsync();
        }

        public Task<StockList?> GetByIdAsync(int id, bool includeInactive = false)
        {
            var q = _context.StockList
                .AsNoTracking()
                .Where(x => x.Id == id);

            if (!includeInactive)
                q = q.Where(x => x.IsActive);

            return q.FirstOrDefaultAsync();
        }

        public async Task AddAsync(StockList item)
        {
            _context.StockList.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StockList item)
        {
            _context.StockList.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task SetActiveAsync(int id, bool isActive)
        {
            var item = await _context.StockList.FirstOrDefaultAsync(x => x.Id == id);
            if (item is null) return;

            item.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.StockList.FirstOrDefaultAsync(x => x.Id == id);
            if (item is null) return;

            _context.StockList.Remove(item);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsByStockUnitIdAsync(int unitId, int? excludeId = null,  bool includeInactive = true)
        {
            var query = _context.StockList.Where(x => x.StockUnitId == unitId);

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return query.AnyAsync();
        }
    }
}
