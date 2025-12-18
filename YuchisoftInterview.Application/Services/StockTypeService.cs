using YuchisoftTest.Application.Interface.Repository;
using YuchisoftTest.Application.Interface.Service;
using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Services
{
    public class StockTypeService : IStockTypeService
    {
        private readonly IStockTypeRepository _stockTypeRepository;

        public StockTypeService(IStockTypeRepository stockTypeRepository)
        {
            _stockTypeRepository = stockTypeRepository;
        }

        public Task<List<StockType>> GetAllAsync(bool includeInactive = false)
        {
            return _stockTypeRepository.GetAllAsync(includeInactive);
        }

        public Task<StockType?> GetByIdAsync(int id, bool includeInactive = false)
        {
            return _stockTypeRepository.GetByIdAsync(id, includeInactive);
        }

        public async Task CreateAsync(StockType item)
        {
            Validate(item, true);

            var exists = await _stockTypeRepository.ExistsByNameAsync(item.Name);
            if (exists) throw new InvalidOperationException("UnitCode must be unique.");

            await _stockTypeRepository.AddAsync(item);
        }

        public async Task UpdateAsync(StockType item)
        {
            Validate(item, false);

            var exists = await _stockTypeRepository.ExistsByNameAsync(item.Name, item.Id, true);
            if (exists) throw new InvalidOperationException("UnitCode must be unique.");

            item.UpdatedAt = DateTime.Now;
            await _stockTypeRepository.UpdateAsync(item);
        }

        public Task DeleteAsync(int id)
        {
            return _stockTypeRepository.DeleteAsync(id);
        }

        public Task SetActiveAsync(int id, bool isActive)
        {
            return _stockTypeRepository.SetActiveAsync(id, isActive);
        }

        private static void Validate(StockType item, bool isCreate)
        {
            if (!isCreate && item.Id <= 0)
                throw new ArgumentException("Id is required.");

            if (string.IsNullOrWhiteSpace(item.Name))
                throw new ArgumentException("Name is required.");
        }
    }
}
