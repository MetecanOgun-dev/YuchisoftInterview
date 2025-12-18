using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Interface.Service
{
    public interface IStockTypeService
    {
        Task<List<StockType>> GetAllAsync(bool includeInactive = false);
        Task<StockType?> GetByIdAsync(int id, bool includeInactive = false);

        Task CreateAsync(StockType item);
        Task UpdateAsync(StockType item);

        Task SetActiveAsync(int id, bool isActive);
        Task DeleteAsync(int id);
    }
}
