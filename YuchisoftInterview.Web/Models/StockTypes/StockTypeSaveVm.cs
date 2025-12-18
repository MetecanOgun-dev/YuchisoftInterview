using System.ComponentModel.DataAnnotations;
using static YuchisoftTest.Domain.Enums.Enums;

namespace YuchisoftTest.Web.Models.StockTypes;

public class StockTypeSaveVm
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = null!;

    [Required]
    public StockClass StockClass { get; set; }
}