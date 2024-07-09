using MediatR;
using InventoryManagement.Data;
using InventoryManagement.Features.Queries;
using InventoryManagement.Features.DTOs;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Features.Handlers
{
    public class GetDailyConsumptionHandler : IRequestHandler<GetDailyConsumptionQuery, IEnumerable<ConsumptionLogDto>>
    {
        private readonly InventoryDbContext _context;

        public GetDailyConsumptionHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConsumptionLogDto>> Handle(GetDailyConsumptionQuery request, CancellationToken cancellationToken)
        {
            var date = new DateTime(request.Year, request.Month, request.Day);

            var consumptionLogs = await _context.ConsumptionLogs
                .Include(log => log.Product)
                .Where(log => log.Date.Date == date.Date)
                .ToListAsync(cancellationToken);

            var groupedLogs = consumptionLogs
                .GroupBy(log => new { log.ProductId, log.Product.Name })
                .Select(g => new ConsumptionLogDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    QuantityConsumed = g.Sum(log => log.QuantityConsumed),
                    TotalCost = g.Sum(log => log.TotalCost),
                    Date = date
                })
                .ToList();

            return groupedLogs;
        }
    }
}
