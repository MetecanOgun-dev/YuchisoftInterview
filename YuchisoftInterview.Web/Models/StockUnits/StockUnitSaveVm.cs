using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static YuchisoftTest.Domain.Enums.Enums;

namespace YuchisoftTest.Web.Models.StockUnits;

public class StockUnitSaveVm
{
    public int Id { get; set; }

    [Required]
    public int StockTypeId { get; set; }

    [Required, MaxLength(50)]
    public string UnitCode { get; set; } = null!;

    [Required]
    public QuantityUnit QuantityUnit { get; set; }

    [Required, MaxLength(250)]
    public string Description { get; set; } = null!;

    [Required]
    public decimal PurchasePrice { get; set; }

    [Required]
    public Currency PurchaseCurrency { get; set; }

    public decimal? SalePrice { get; set; }

    public Currency SaleCurrency { get; set; }

    // dropdown data
    public List<SelectListItem> StockTypeOptions { get; set; } = new();
}
