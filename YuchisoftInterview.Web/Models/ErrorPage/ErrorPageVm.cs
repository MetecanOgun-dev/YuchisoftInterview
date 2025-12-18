using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Web.Models.ErrorPage
{
    public class ErrorPageVm
    {
        public string? TraceId { get; set; }
        public List<ErrorLog> Logs { get; set; } = new();
    }
}
