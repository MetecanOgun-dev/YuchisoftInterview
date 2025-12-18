using Microsoft.AspNetCore.Mvc;
using YuchisoftTest.Application.Interfaces.Repositories;
using YuchisoftTest.Web.Models.ErrorPage;

namespace YuchisoftTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IErrorLogRepository _errorLogRepository;

        public HomeController(IErrorLogRepository errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Error(string traceId)
        {
            if (string.IsNullOrWhiteSpace(traceId))
                return View(new ErrorPageVm { TraceId = HttpContext.TraceIdentifier });

            var logs = await _errorLogRepository.GetByTraceIdAsync(traceId);

            return View(new ErrorPageVm
            {
                TraceId = traceId,
                Logs = logs
            });
        }
    }
}