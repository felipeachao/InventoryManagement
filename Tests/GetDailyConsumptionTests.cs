using Xunit;
using InventoryManagement.Features.Handlers;
using InventoryManagement.Features.Queries;
using InventoryManagement.Data.Entities;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Tests
{
    public class GetDailyConsumptionTests
    {
        private readonly InventoryDbContext _context;
        private readonly GetDailyConsumptionHandler _handler;

        public GetDailyConsumptionTests()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new InventoryDbContext(options);
            _handler = new GetDailyConsumptionHandler(_context);
        }

        [Fact]
        public async Task Should_Get_Daily_Consumption()
        {
            var product = new Product
            {
                PartNumber = "12345",
                Name = "Test Product",
                CostPrice = 10.5m,
                StockQuantity = 100
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var consumptionLog = new ConsumptionLog
            {
                ProductId = product.Id,
                QuantityConsumed = 10,
                Date = DateTime.UtcNow,
                TotalCost = 105m
            };

            _context.ConsumptionLogs.Add(consumptionLog);
            await _context.SaveChangesAsync();

            var today = DateTime.UtcNow;

            var query = new GetDailyConsumptionQuery
            {
                Day = today.Day,
                Month = today.Month,
                Year = today.Year
            };

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(10, result.First().QuantityConsumed);
            Assert.Equal(105m, result.First().TotalCost);
        }
    }
}
