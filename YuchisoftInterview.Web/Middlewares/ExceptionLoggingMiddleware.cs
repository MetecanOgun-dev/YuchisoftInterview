using YuchisoftTest.Application.Interfaces.Repositories;
using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Web.Middlewares
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IErrorLogRepository _errorRepository)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted) 
                    throw;

                var traceId = context.TraceIdentifier;

                try
                {
                    var log = new ErrorLog
                    {
                        TraceId = traceId,
                        ExceptionType = ex.GetType().FullName ?? ex.GetType().Name,
                        Message = ex.Message,
                        StackTrace = ex.ToString(),
                        Method = context.Request.Method,
                        Path = context.Request.Path.ToString(),
                        QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null,
                        CreatedAt = DateTime.Now
                    };

                    await _errorRepository.AddAsync(log);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "ErrorLog failed. TraceId={TraceId}", traceId);
                }

                _logger.LogError(ex, "Unhandled exception. TraceId={TraceId}", traceId);

                var url = $"/Home/Error?traceId={Uri.EscapeDataString(traceId)}";
                context.Response.Redirect(url);
                return;
            }
        }
    }
}
