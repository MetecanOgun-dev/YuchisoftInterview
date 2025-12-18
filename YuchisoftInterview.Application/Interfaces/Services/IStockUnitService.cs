using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Interfaces.Services
{
    public interface IStockUnitService
    {
        Task<List<StockUnit>> GetAllAsync(bool includeInactive = false);
        Task<StockUnit?> GetByIdAsync(int id, bool includeInactive = false);

        Task CreateAsync(StockUnit item);
        Task UpdateAsync(StockUnit item);

        Task SetActiveAsync(int id, bool isActive);
        Task DeleteAsync(int id);
    }
}
