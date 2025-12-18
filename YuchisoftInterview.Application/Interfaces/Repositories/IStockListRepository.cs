using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Interface.Repository
{
    public interface IStockListRepository
    {
        Task<List<StockList>> GetAllAsync(bool includeInactive = false);
        Task<StockList?> GetByIdAsync(int id, bool includeInactive = false);
        Task AddAsync(StockList item);
        Task UpdateAsync(StockList item);
        Task SetActiveAsync(int id, bool isActive);
        Task DeleteAsync(int id);
        Task<bool> ExistsByStockUnitIdAsync(int stockUnitId, int? excludeId = null, bool includeInactive = true);
    }
}
