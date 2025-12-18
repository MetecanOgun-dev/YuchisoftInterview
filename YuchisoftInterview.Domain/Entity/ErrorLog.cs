using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuchisoftTest.Domain.Entity
{
    public class ErrorLog
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string TraceId { get; set; } = null!;

        [Required, MaxLength(200)]
        public string ExceptionType { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Message { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string? StackTrace { get; set; }

        [Required, MaxLength(10)]
        public string Method { get; set; } = null!;

        [Required, MaxLength(500)]
        public string Path { get; set; } = null!;

        [MaxLength(2000)]
        public string? QueryString { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
