using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuchisoftTest.Domain.Entity.Base;
using static YuchisoftTest.Domain.Enums.Enums;

namespace YuchisoftTest.Domain.Entity
{
    public class StockUnit : EntityBase
    {
        [Required]
        public int StockTypeId { get; set; }

        public StockType StockType { get; set; } = null!;

        public StockList? StockList { get; set; }

        [Required]
        [MaxLength(50)]
        public string UnitCode { get; set; } = null!;

        [Required]
        public QuantityUnit QuantityUnit { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Required]
        public Currency PurchaseCurrency { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }
           
        public Currency SaleCurrency { get; set; }
    }
}
