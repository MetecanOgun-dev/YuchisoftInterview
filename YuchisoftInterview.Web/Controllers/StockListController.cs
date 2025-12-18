using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using YuchisoftTest.Application.Interfaces.Services;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Web.Models.StockList;

namespace YuchisoftTest.Web.Controllers
{
    public class StockListController : Controller
    {
        private readonly IStockListService _stockListService;
        private readonly IStockUnitService _stockUnitService;

        public StockListController(IStockListService stockListService, IStockUnitService stockUnitService)
        {
            _stockListService = stockListService;
            _stockUnitService = stockUnitService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _stockListService.GetAllAsync(includeInactive: true);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> SaveModal(int id = 0)
        {
            var vm = new StockListSaveVm();

            if (id > 0)
            {
                var entity = await _stockListService.GetByIdAsync(id, includeInactive: true);
                if (entity is null) return NotFound();

                vm.Id = entity.Id;
                vm.StockUnitId = entity.StockUnitId;
                vm.Quantity = entity.Quantity;
                vm.CriticalQuantity = entity.CriticalQuantity;
                vm.ShelfInfo = entity.ShelfInfo;
                vm.CabinetInfo = entity.CabinetInfo;
            }

            vm.StockUnitOptions = await GetStockUnitOptions();
            return PartialView("_SaveModalBody", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveStockList(StockListSaveVm vm)
        {
            vm.StockUnitOptions = await GetStockUnitOptions();

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return PartialView("_SaveModalBody", vm);
            }

            try
            {
                if (vm.Id == 0)
                {
                    var entity = new StockList
                    {
                        StockUnitId = vm.StockUnitId,
                        Quantity = vm.Quantity,
                        CriticalQuantity = vm.CriticalQuantity,
                        ShelfInfo = vm.ShelfInfo,
                        CabinetInfo = vm.CabinetInfo
                    };

                    await _stockListService.CreateAsync(entity);
                    return Json(await BuildRowJsonAsync("create", entity));
                }
                else
                {
                    var db = await _stockListService.GetByIdAsync(vm.Id, includeInactive: true);
                    if (db is null) return NotFound();

                    db.StockUnitId = vm.StockUnitId;
                    db.Quantity = vm.Quantity;
                    db.CriticalQuantity = vm.CriticalQuantity;
                    db.ShelfInfo = vm.ShelfInfo;
                    db.CabinetInfo = vm.CabinetInfo;

                    await _stockListService.UpdateAsync(db);
                    return Json(await BuildRowJsonAsync("update", db));
                } 
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(vm.StockUnitId), ex.Message);
                Response.StatusCode = 400;
                return PartialView("_SaveModalBody", vm);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Response.StatusCode = 400;
                return PartialView("_SaveModalBody", vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStockList(int id)
        {
            await _stockListService.DeleteAsync(id);
            return Json(new { ok = true, id });
        }

        private async Task<List<SelectListItem>> GetStockUnitOptions()
        {
            var units = await _stockUnitService.GetAllAsync();

            return units.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.UnitCode} - {u.Description}"
            }).ToList();
        }

        private async Task<object> BuildRowJsonAsync(string mode, StockList sl)
        {
            var unit = await _stockUnitService.GetByIdAsync(sl.StockUnitId, includeInactive: true);
            
            return new
            {
                mode,
                id = sl.Id,
                stockUnitId = sl.StockUnitId,
                unitCode = unit.UnitCode,
                description = unit.Description,
                stockTypeName = unit.StockType?.Name,
                quantityUnit = unit.QuantityUnit.ToString(),
                quantity = sl.Quantity,
                criticalQuantity = sl.CriticalQuantity,
                shelfInfo = sl.ShelfInfo,
                cabinetInfo = sl.CabinetInfo,
                isActive = sl.IsActive
            };
        }
    }
}
