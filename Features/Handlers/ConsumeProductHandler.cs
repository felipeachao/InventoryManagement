using MediatR;
using InventoryManagement.Data;
using InventoryManagement.Features.Commands;
using InventoryManagement.Data.Entities;
using InventoryManagement.Exceptions;

namespace InventoryManagement.Features.Handlers
{
    public class ConsumeProductHandler : IRequestHandler<ConsumeProductCommand, bool>
    {
        private readonly InventoryDbContext _context;

        public ConsumeProductHandler(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ConsumeProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                throw new NotFoundException($"Product with id {request.ProductId} not found");
            }

            if (product.StockQuantity < request.Quantity)
            {
                throw new InvalidOperationException("Insufficient stock available");
            }

            product.StockQuantity -= request.Quantity;

            var consumptionLog = new ConsumptionLog
            {
                ProductId = request.ProductId,
                QuantityConsumed = request.Quantity,
                Date = DateTime.UtcNow,
                TotalCost = request.Quantity * product.CostPrice
            };

            _context.ConsumptionLogs.Add(consumptionLog);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
