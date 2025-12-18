using Microsoft.AspNetCore.Mvc;
using YuchisoftTest.Application.Interface.Service;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Web.Models.StockTypes;
using YuchisoftTest.Web.Models.StockUnits;

namespace YuchisoftTest.Web.Controllers
{
    public class StockTypeController : Controller
    {
        private readonly IStockTypeService _service;

        public StockTypeController(IStockTypeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _service.GetAllAsync(includeInactive: true);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> SaveModal(int id = 0)
        {
            var vm = new StockTypeSaveVm();
            if (id > 0)
            {
                var entity = await _service.GetByIdAsync(id, includeInactive: true);
                if (entity is null) return NotFound();
                
                vm.Id = entity.Id;
                vm.Name = entity.Name;
                vm.StockClass = entity.StockClass;

            }
            return PartialView("_SaveModalBody", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveStockType(StockTypeSaveVm vm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return PartialView("_SaveModalBody", vm);
            }
            try
            {
                if (vm.Id == 0)
                {
                    var entity = new StockType
                    {
                        Name = vm.Name,
                        StockClass = vm.StockClass
                    };
                    await _service.CreateAsync(entity);
                    return Json(await BuildUnitRowJsonAsync("create", entity));
                }
                else
                {
                    var entity = await _service.GetByIdAsync(vm.Id, includeInactive: true);
                    if (entity is null) return NotFound();

                    entity.Name = vm.Name;
                    entity.StockClass = vm.StockClass;

                    await _service.UpdateAsync(entity);
                    return Json(await BuildUnitRowJsonAsync("update", entity));
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(vm.Name), ex.Message);
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
        public async Task<IActionResult> DeleteStockType(int id)
        {
            await _service.DeleteAsync(id);
            return Json(new { ok = true, id });
        }
        private async Task<object> BuildUnitRowJsonAsync(string mode, StockType stockType)
        {
            return new
            {
                mode,
                id = stockType.Id,
                name = stockType.Name,
                stockClass = stockType.StockClass.ToString(),
                isActive = stockType.IsActive
            };
        }
    }
}
