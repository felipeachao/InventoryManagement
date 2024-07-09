using InventoryManagement.Data.Entities;

namespace InventoryManagement.Data.Repositories
{
    public interface IProductRepository
    {
        Task<int> CreateProductAsync(Product product);
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}
