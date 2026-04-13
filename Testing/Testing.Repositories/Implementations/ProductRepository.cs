using Microsoft.EntityFrameworkCore;
using Testing.Domain.Entities;
using Testing.Repositories.Interfaces;

namespace Testing.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbPrintedBoardsContext _context;

        public ProductRepository(DbPrintedBoardsContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllActiveAsync()
        {
            return await _context.Products
                .Include(p => p.PcbSpecifications)
                .Where(p => p.IsActive == true)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Products
            .Include(p => p.PcbSpecifications)
            .Where(p => p.IdCategory == categoryId && p.IsActive == true)
            .ToListAsync();
        }

        public async Task<Product> GetByIdWithSpecAsync(int id)
        {
            return await _context.Products
            .Include(p => p.PcbSpecifications)
            .FirstOrDefaultAsync(p => p.IdProduct == id && p.IsActive == true);
        }

        public async Task<List<Product>> GetPopularAsync(int count)
        {
            return await _context.Products
            .Include(p => p.PcbSpecifications)
            .Where(p => p.IsActive == true)
            .Take(count)
            .ToListAsync();
        }
    }
}
