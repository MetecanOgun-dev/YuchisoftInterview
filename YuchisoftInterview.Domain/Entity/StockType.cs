using System.ComponentModel.DataAnnotations;
using YuchisoftTest.Domain.Entity.Base;
using static YuchisoftTest.Domain.Enums.Enums;

namespace YuchisoftTest.Domain.Entity
{
    public class StockType : EntityBase
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        public StockClass StockClass { get; set; }

        public ICollection<StockUnit> StockUnits { get; set; } = new List<StockUnit>();
    }
}
