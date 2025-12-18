using Microsoft.EntityFrameworkCore;
using YuchisoftTest.Application.Interface.Repository;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Infrastructure.Persistence;

namespace YuchisoftTest.Infrastructure.Repositories
{
    public class StockTypeRepository : IStockTypeRepository
    {
        private readonly AppDbContext _context;

        public StockTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<StockType>> GetAllAsync(bool includeInactive = false)
        {
            var query = _context.StockTypes.AsNoTracking();

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            return query.OrderBy(x => x.Name).ToListAsync();
        }

        public Task<StockType?> GetByIdAsync(int id, bool includeInactive = false)
        {
            var query = _context.StockTypes.AsNoTracking().Where(x => x.Id == id);

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            return query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(StockType item)
        {
            _context.StockTypes.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StockType item)
        {
            _context.StockTypes.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task SetActiveAsync(int id, bool isActive)
        {
            var item = await _context.StockTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (item is null) return;

            item.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
                var item = await _context.StockTypes.FirstOrDefaultAsync(x => x.Id == id);
                if (item is null) return;

                _context.StockTypes.Remove(item);
                await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsByNameAsync(string name, int? excludeId = null, bool includeInactive = false)
        {
            var query = _context.StockTypes.AsNoTracking().Where(x => x.Name.ToLower() == name.ToLower());

            if (!includeInactive)
                query = query.Where(x => x.IsActive);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return query.AnyAsync();
        }

        public Task<StockType?> GetByNameAsync(string name, bool includeInactive = false)
        {
            var q = _context.StockTypes.AsNoTracking().Where(x => x.Name.ToLower() == name.ToLower());

            if (!includeInactive)
                q = q.Where(x => x.IsActive);

            return q.FirstOrDefaultAsync();
        }
    }
}
