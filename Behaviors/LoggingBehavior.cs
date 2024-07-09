using MediatR;
using InventoryManagement.Data;
using InventoryManagement.Data.Entities;

namespace InventoryManagement.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly InventoryDbContext _context;
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(InventoryDbContext context, ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Request: {typeof(TRequest).Name} failed");
                await LogError(ex);
                throw;
            }
        }

        private async Task LogError(Exception ex)
        {
            var log = new ErrorLog
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace ?? "No stack trace",
                Date = DateTime.UtcNow
            };

            _context.ErrorLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
