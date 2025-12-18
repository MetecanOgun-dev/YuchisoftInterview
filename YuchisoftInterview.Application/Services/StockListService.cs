using YuchisoftTest.Application.Interface.Repository;
using YuchisoftTest.Application.Interfaces.Services;
using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Services
{
    public class StockListService : IStockListService
    {
        private readonly IStockListRepository _stockListRepository;

        public StockListService(IStockListRepository stockListRepository)
        {
            _stockListRepository = stockListRepository;
        }
        
        public Task<List<StockList>> GetAllAsync(bool includeInactive = false)
        {
            return _stockListRepository.GetAllAsync(includeInactive);
        }

        public Task<StockList?> GetByIdAsync(int id, bool includeInactive = false)
        {
            return _stockListRepository.GetByIdAsync(id, includeInactive);
        }

        public async Task CreateAsync(StockList item)
        {
            Validate(item, true);

            var exists = await _stockListRepository.ExistsByStockUnitIdAsync(item.StockUnitId, item.Id ,true);
            if (exists) throw new InvalidOperationException("UnitId must be unique.");

            await _stockListRepository.AddAsync(item);
        }

        public async Task UpdateAsync(StockList item)
        {
            Validate(item, false);

            var exists = await _stockListRepository.ExistsByStockUnitIdAsync(item.StockUnitId, item.Id, true);
            if (exists) throw new InvalidOperationException("UnitId must be unique.");

            item.UpdatedAt = DateTime.Now;
            await _stockListRepository.UpdateAsync(item);
        }

        public Task DeleteAsync(int id)
        {
            return _stockListRepository.DeleteAsync(id);
        }

        public Task SetActiveAsync(int id, bool isActive)
        {
            return _stockListRepository.SetActiveAsync(id, isActive);
        }

        private static void Validate(StockList item, bool isCreate)
        {
            if (!isCreate && item.Id <= 0)
                throw new ArgumentException("Id is required.");

            if (item.StockUnitId <= 0)
                throw new ArgumentException("StockUnit is required.");

            if (item.Quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            if (item.CriticalQuantity < 0)
                throw new ArgumentException("CriticalQuantity cannot be negative.");

            if (string.IsNullOrWhiteSpace(item.ShelfInfo))
                throw new ArgumentException("ShelfInfo is required.");

            if (string.IsNullOrWhiteSpace(item.CabinetInfo))
                throw new ArgumentException("CabinetInfo is required.");
        }
    }
}
