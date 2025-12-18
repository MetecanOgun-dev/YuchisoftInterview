using YuchisoftTest.Application.Interface.Repository;
using YuchisoftTest.Application.Interfaces.Services;
using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Services
{
    public class StockUnitService : IStockUnitService
    {
        private readonly IStockUnitRepository _stockUnitRepository;

        public StockUnitService(IStockUnitRepository stockUnitRepository)
        {
            _stockUnitRepository = stockUnitRepository;
        }

        public Task<List<StockUnit>> GetAllAsync(bool includeInactive = false)
        {
            return _stockUnitRepository.GetAllAsync(includeInactive);
        }

        public Task<StockUnit?> GetByIdAsync(int id, bool includeInactive = false)
        {
            return _stockUnitRepository.GetByIdAsync(id, includeInactive);
        }

        public async Task CreateAsync(StockUnit item)
        {
            Validate(item, true);

            var exists = await _stockUnitRepository.ExistsByUnitCodeAsync(item.UnitCode, item.Id, true);
            if (exists) throw new InvalidOperationException("UnitCode must be unique.");

            await _stockUnitRepository.AddAsync(item);
        }
        public async Task UpdateAsync(StockUnit item)
        {
            Validate(item, false);

            var exists = await _stockUnitRepository.ExistsByUnitCodeAsync(item.UnitCode, item.Id, true);
            if (exists) throw new InvalidOperationException("UnitCode must be unique.");

            item.UpdatedAt = DateTime.Now;
            await _stockUnitRepository.UpdateAsync(item);
        }

        public Task DeleteAsync(int id)
        {
            return _stockUnitRepository.DeleteAsync(id);
        }

        public Task SetActiveAsync(int id, bool isActive)
        {
            return _stockUnitRepository.SetActiveAsync(id, isActive);
        }

        private static void Validate(StockUnit item, bool isCreate)
        {
            if (!isCreate && item.Id <= 0)
                throw new ArgumentException("Id is required.");

            if (item.StockTypeId <= 0)
                throw new ArgumentException("StockType is required.");

            if (string.IsNullOrWhiteSpace(item.UnitCode))
                throw new ArgumentException("UnitCode is required.");

            if (string.IsNullOrWhiteSpace(item.Description))
                throw new ArgumentException("Description is required.");

            if (item.PurchasePrice < 0)
                throw new ArgumentException("PurchasePrice cannot be negative.");

            if (item.SalePrice.HasValue && item.SalePrice.Value < 0)
                throw new ArgumentException("SalePrice cannot be negative.");
        }
    }
}
