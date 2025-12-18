using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Interfaces.Services
{
    public interface IStockListService
    {
        Task<List<StockList>> GetAllAsync(bool includeInactive = false);
        Task<StockList?> GetByIdAsync(int id, bool includeInactive = false);

        Task CreateAsync(StockList item);
        Task UpdateAsync(StockList item);

        Task DeleteAsync(int id);
        Task SetActiveAsync(int id, bool isActive);
    }
}
