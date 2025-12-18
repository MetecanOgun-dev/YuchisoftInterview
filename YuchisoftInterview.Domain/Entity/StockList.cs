using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuchisoftTest.Domain.Entity.Base;

namespace YuchisoftTest.Domain.Entity
{
    public class StockList : EntityBase
    {
        [Required]
        public int StockUnitId { get; set; }

        public StockUnit StockUnit { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CriticalQuantity { get; set; }

        [Required]
        [MaxLength(100)]
        public string ShelfInfo { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string CabinetInfo { get; set; } = null!;

    }
}
