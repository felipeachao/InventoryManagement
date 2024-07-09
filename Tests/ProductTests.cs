using Xunit;
using InventoryManagement.Features.Commands;
using InventoryManagement.Features.Handlers;
using InventoryManagement.Features.Queries;
using InventoryManagement.Data.Entities;
using InventoryManagement.Data.Repositories;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Tests
{
    public class ProductTests
    {
        private readonly IProductRepository _repository;
        private readonly ProductHandler _handler;
        private readonly InventoryDbContext _context;

        public ProductTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new InventoryDbContext(options);

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A string de conexão não pode ser nula ou vazia.");
            }

            _repository = new ProductRepository(connectionString);
            _handler = new ProductHandler(_repository, _context);
        }

        [Fact]
        public async Task Should_Create_Product()
        {
            var command = new CreateProductCommand
            {
                PartNumber = "12345",
                Name = "Test Product",
                CostPrice = 10.5m,
                StockQuantity = 100
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result > 0);
        }

        [Fact]
        public async Task Should_Get_Product_By_Id()
        {
            var product = new Product
            {
                PartNumber = "12345",
                Name = "Test Product",
                CostPrice = 10.5m,
                StockQuantity = 100
            };

            var productId = await _repository.CreateProductAsync(product);

            var query = new GetProductByIdQuery(productId);
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task Should_Update_Product()
        {
            var product = new Product
            {
                PartNumber = "12345",
                Name = "Test Product",
                CostPrice = 10.5m,
                StockQuantity = 100
            };

            var productId = await _repository.CreateProductAsync(product);

            var command = new UpdateProductCommand
            {
                Id = productId,
                PartNumber = "67890",
                Name = "Updated Product",
                CostPrice = 12.5m,
                StockQuantity = 200
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);

            var updatedProduct = await _repository.GetProductByIdAsync(productId);
            Assert.NotNull(updatedProduct);
            Assert.Equal("67890", updatedProduct?.PartNumber);
            Assert.Equal("Updated Product", updatedProduct?.Name);
        }

        [Fact]
        public async Task Should_Delete_Product()
        {
            var product = new Product
            {
                PartNumber = "12345",
                Name = "Test Product",
                CostPrice = 10.5m,
                StockQuantity = 100
            };

            var productId = await _repository.CreateProductAsync(product);

            var command = new DeleteProductCommand(productId);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);

            var deletedProduct = await _repository.GetProductByIdAsync(productId);
            Assert.Null(deletedProduct);
        }
    }
}
