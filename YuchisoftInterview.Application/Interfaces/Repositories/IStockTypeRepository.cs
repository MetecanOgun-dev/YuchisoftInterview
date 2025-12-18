using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Interface.Repository
{
    public interface IStockTypeRepository
    {
        Task<List<StockType>> GetAllAsync(bool includeInactive = false);
        Task<StockType?> GetByIdAsync(int id, bool includeInactive = false);
        Task AddAsync(StockType item);
        Task UpdateAsync(StockType item);
        Task SetActiveAsync(int id, bool isActive);
        Task DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null, bool includeInactive = true);
        Task<StockType?> GetByNameAsync(string name, bool includeInactive = true);
    }
}
