using test_e4.Models;

namespace test_e4.BusinessLayer.Interfaces
{
    public interface IProductsServices
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task<Product> UpdateAsync(int id, Product newProduct); 
        Task DeleteAsync(int id);
    }
}
