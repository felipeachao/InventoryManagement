using Xunit;
using InventoryManagement.Features.Commands;
using InventoryManagement.Features.Handlers;
using InventoryManagement.Data.Entities;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Exceptions;

namespace InventoryManagement.Tests
{
    public class ConsumeProductTests
    {
        private readonly InventoryDbContext _context;
        private readonly ConsumeProductHandler _handler;

        public ConsumeProductTests()
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new InventoryDbContext(options);
            _handler = new ConsumeProductHandler(_context);
        }

        [Fact]
        public async Task Should_Consume_Product()
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

            var command = new ConsumeProductCommand
            {
                ProductId = product.Id,
                Quantity = 10
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);

            var updatedProduct = await _context.Products.FindAsync(product.Id);
            Assert.NotNull(updatedProduct); 
            Assert.Equal(90, updatedProduct!.StockQuantity);
        }

        [Fact]
        public async Task Should_Not_Consume_Product_When_Insufficient_Stock()
        {
            var product = new Product
            {
                PartNumber = "12345",
                Name = "Test Product",
                CostPrice = 10.5m,
                StockQuantity = 5
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var command = new ConsumeProductCommand
            {
                ProductId = product.Id,
                Quantity = 10
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Should_Not_Consume_Nonexistent_Product()
        {
            var command = new ConsumeProductCommand
            {
                ProductId = 999,
                Quantity = 10
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
