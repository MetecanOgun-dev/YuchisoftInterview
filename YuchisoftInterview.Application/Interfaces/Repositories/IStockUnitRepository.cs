using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Interface.Repository
{
    public interface IStockUnitRepository
    {
        Task<List<StockUnit>> GetAllAsync(bool includeInactive = false);
        Task<StockUnit?> GetByIdAsync(int id, bool includeInactive = false);
        Task AddAsync(StockUnit item);
        Task UpdateAsync(StockUnit item);
        Task SetActiveAsync(int id, bool isActive);
        Task DeleteAsync(int id);
        Task<StockUnit?> GetByUnitCodeAsync(string unitCode, bool includeInactive = false);
        Task<bool> ExistsByUnitCodeAsync(string unitCode, int? excludeId = null, bool includeInactive = false);
    }
}
