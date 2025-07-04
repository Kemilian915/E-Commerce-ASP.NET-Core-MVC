using Microsoft.EntityFrameworkCore;
using test_e4.BusinessLayer.Interfaces;
using test_e4.Data;
using test_e4.Models;

namespace test_e4.BusinessLayer.Services
{
    public class ProductsServices : IProductsServices
    {
        private readonly AppDbContext _context;
        public ProductsServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Products.FirstOrDefaultAsync(n => n.Id == id);
            _context.Products.Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var result = await _context.Products
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return result;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var result = await _context.Products.FirstOrDefaultAsync(n => n.Id == id);
            return result;
        }

        public async Task<Product> UpdateAsync(int id, Product newProduct)
        {
            _context.Update(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }
    }
}