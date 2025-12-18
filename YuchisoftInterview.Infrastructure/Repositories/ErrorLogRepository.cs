using Microsoft.EntityFrameworkCore;
using YuchisoftTest.Application.Interfaces.Repositories;
using YuchisoftTest.Domain.Entity;
using YuchisoftTest.Infrastructure.Persistence;

namespace YuchisoftTest.Infrastructure.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly IDbContextFactory<AppDbContext> _factory;
        private readonly AppDbContext _context;
        public ErrorLogRepository(IDbContextFactory<AppDbContext> factory, AppDbContext context)
        {
            _factory = factory;
            _context = context;
        }

        public async Task AddAsync(ErrorLog log)
        {
            await using var _newContext = await _factory.CreateDbContextAsync();
            _newContext.ErrorLogs.Add(log);
            await _newContext.SaveChangesAsync();
        }

        public async Task<List<ErrorLog>> GetByTraceIdAsync(string traceId)
        {
            return await _context.ErrorLogs
                .Where(x => x.TraceId == traceId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
