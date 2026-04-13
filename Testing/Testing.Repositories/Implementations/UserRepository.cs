using Testing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Testing.Repositories.Interfaces;

namespace Testing.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DbPrintedBoardsContext _context;

        public UserRepository(DbPrintedBoardsContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);    
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserSessions)  
                .FirstOrDefaultAsync(u => u.IdUser == id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            return await _context.Countrys.ToListAsync();   
        }
    }
}
