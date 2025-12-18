using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using YuchisoftTest.Application.Interface.Service;
using YuchisoftTest.Application.Interfaces.Services;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Web.Models.StockUnits;

namespace YuchisoftTest.Web.Controllers
{
    public class StockUnitController : Controller
    {
        private readonly IStockUnitService _unitService;
        private readonly IStockTypeService _typeService;

        public StockUnitController(IStockUnitService unitService, IStockTypeService typeService)
        {
            _unitService = unitService;
            _typeService = typeService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _unitService.GetAllAsync(includeInactive: true);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> SaveModal(int id = 0)
        {
            var vm = new StockUnitSaveVm();

            if (id > 0)
            {
                var entity = await _unitService.GetByIdAsync(id, includeInactive: true);
                if (entity is null) return NotFound();

                vm.Id = entity.Id;
                vm.StockTypeId = entity.StockTypeId;
                vm.UnitCode = entity.UnitCode;
                vm.QuantityUnit = entity.QuantityUnit;
                vm.Description = entity.Description;
                vm.PurchasePrice = entity.PurchasePrice;
                vm.PurchaseCurrency = entity.PurchaseCurrency;
                vm.SalePrice = entity.SalePrice;
                vm.SaleCurrency = entity.SaleCurrency;
            }

            vm.StockTypeOptions = await GetStockTypeOptions();
            return PartialView("_SaveModalBody", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveStockUnit(StockUnitSaveVm vm)
        {
            vm.StockTypeOptions = await GetStockTypeOptions();

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return PartialView("_SaveModalBody", vm);
            }

            try
            {
                if (vm.Id == 0)
                {
                    var entity = new StockUnit
                    {
                        StockTypeId = vm.StockTypeId,
                        UnitCode = vm.UnitCode,
                        QuantityUnit = vm.QuantityUnit,
                        Description = vm.Description,
                        PurchasePrice = vm.PurchasePrice,
                        PurchaseCurrency = vm.PurchaseCurrency,
                        SalePrice = vm.SalePrice,
                        SaleCurrency = vm.SaleCurrency
                    };
                    await _unitService.CreateAsync(entity);
                    return Json(await BuildUnitRowJsonAsync("create", entity));
                }
                else
                {
                    var db = await _unitService.GetByIdAsync(vm.Id, includeInactive: true);
                    if (db is null) 
                        return NotFound();

                    db.StockTypeId = vm.StockTypeId;
                    db.UnitCode = vm.UnitCode;
                    db.QuantityUnit = vm.QuantityUnit;
                    db.Description = vm.Description;
                    db.PurchasePrice = vm.PurchasePrice;
                    db.PurchaseCurrency = vm.PurchaseCurrency;
                    db.SalePrice = vm.SalePrice;
                    db.SaleCurrency = vm.SaleCurrency;

                    await _unitService.UpdateAsync(db);
                    return Json(await BuildUnitRowJsonAsync("update", db));
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(vm.UnitCode), ex.Message);
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
        public async Task<IActionResult> DeleteStockUnit(int id)
        {
            await _unitService.DeleteAsync(id);
            return Json(new { ok = true, id });
        }

        private async Task<List<SelectListItem>> GetStockTypeOptions()
        {
            var types = await _typeService.GetAllAsync(includeInactive: false);
            return types
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToList();
        }

        private async Task<object> BuildUnitRowJsonAsync(string mode, StockUnit unit)
        {
            var type = await _typeService.GetByIdAsync(unit.StockTypeId, includeInactive: true);

            return new
            {
                mode,
                id = unit.Id,
                unitCode = unit.UnitCode,
                stockTypeName = type.Name,
                quantityUnit = unit.QuantityUnit.ToString(),
                description = unit.Description,
                purchasePrice = unit.PurchasePrice,
                purchaseCurrency = unit.PurchaseCurrency.ToString(),
                salePrice = unit.SalePrice,
                saleCurrency = unit.SaleCurrency.ToString(),
                isActive = unit.IsActive
            };
        }
    }
}
