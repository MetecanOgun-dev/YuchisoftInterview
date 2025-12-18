using System.ComponentModel.DataAnnotations;

namespace YuchisoftTest.Domain.Entity
{
    public class SeedHistory
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Key { get; set; } = "InitialSeed";

        public DateTime AppliedAt { get; set; } = DateTime.Now;
    }
}
