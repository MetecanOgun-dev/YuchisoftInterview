using YuchisoftTest.Domain.Entity;

namespace YuchisoftTest.Application.Interfaces.Repositories
{
    public interface IErrorLogRepository
    {
        Task AddAsync(ErrorLog log);
        Task<List<ErrorLog>> GetByTraceIdAsync(string traceId);
    }
}
