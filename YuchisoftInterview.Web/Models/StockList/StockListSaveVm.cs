using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static YuchisoftTest.Domain.Enums.Enums;

namespace YuchisoftTest.Web.Models.StockList
{
    public class StockListSaveVm
    {
        public int Id { get; set; } // 0=create, >0=update

        [Required]
        public int StockUnitId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal CriticalQuantity { get; set; }

        [Required, MaxLength(100)]
        public string ShelfInfo { get; set; } = null!;

        [Required, MaxLength(100)]
        public string CabinetInfo { get; set; } = null!;

        public List<SelectListItem> StockUnitOptions { get; set; } = new();
    }
}
