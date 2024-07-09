using Dapper;
using InventoryManagement.Data.Entities;
using MySql.Data.MySqlClient; 

namespace InventoryManagement.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "INSERT INTO Products (PartNumber, Name, CostPrice, StockQuantity) VALUES (@PartNumber, @Name, @CostPrice, @StockQuantity); SELECT LAST_INSERT_ID();";
                return await connection.ExecuteScalarAsync<int>(query, product);
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Products WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Products";
                return await connection.QueryAsync<Product>(query);
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "UPDATE Products SET PartNumber = @PartNumber, Name = @Name, CostPrice = @CostPrice, StockQuantity = @StockQuantity WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(query, product);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var query = "DELETE FROM Products WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
        }
    }
}
