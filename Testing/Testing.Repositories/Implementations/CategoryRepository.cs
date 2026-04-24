using Microsoft.EntityFrameworkCore;
using Testing.Domain.Entities;
using Testing.Repositories.Interfaces;

namespace Testing.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbPrintedBoardsContext _context;

        public CategoryRepository(DbPrintedBoardsContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive == true)
                .ToListAsync();
        }
    }
}
