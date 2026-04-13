using Testing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Testing.Repositories.Interfaces;

namespace Testing.Repositories.Implementations
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly DbPrintedBoardsContext _context;

        public UserSessionRepository(DbPrintedBoardsContext context)
        {
            _context = context;
        }

        public async Task<UserSession?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.UserSessions
                .Include(us => us.IdUserNavigation)
                .FirstOrDefaultAsync(us => us.RefreshToken == refreshToken);
        }

        public async Task<UserSession?> GetActiveByRefreshTokenAsync(string refreshToken)
        {
            return await _context.UserSessions
                .Include(us => us.IdUserNavigation)
                .Where(us => us.RefreshToken == refreshToken
                          && us.IsActive == true
                          && us.ExpiresAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task CreateSessionAsync(UserSession session)
        {
            await _context.UserSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateSessionAsync(string refreshToken)
        {
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(us => us.RefreshToken == refreshToken);

            if (session != null)
            {
                session.IsActive = false; 
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeactivateAllUserSessionsAsync(int userId)
        {
            var sessions = await _context.UserSessions
                .Where(us => us.IdUser == userId && us.IsActive == true)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
