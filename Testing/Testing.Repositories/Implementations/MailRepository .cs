using Microsoft.EntityFrameworkCore;
using Testing.Domain.Entities;
using Testing.Repositories.Interfaces;

namespace Testing.Repositories.Implementations
{
    public class MailRepository : IMailRepository
    {

        private readonly DbPrintedBoardsContext _context;

        public MailRepository(DbPrintedBoardsContext context)
        {
            _context = context;
        }

        public async Task DeleteCodeAsync(ConfirmationCode confirmCode)
        {
            _context.ConfirmationCodes.Remove(confirmCode);
            await _context.SaveChangesAsync();
        }

        public async Task<ConfirmationCode?> FindByEmailAsync(string email)
        {
            return await _context.ConfirmationCodes
                .Where(c => c.Email == email)
                .OrderByDescending(c => c.ExpiresIn)
                .FirstOrDefaultAsync();
        }

        public async Task<ConfirmationCode?> VerifyCodeAsync(string email, string code)
        {
            return await _context.ConfirmationCodes
                .FirstOrDefaultAsync(c => c.Email == email && c.Code == code && c.ExpiresIn > DateTime.UtcNow);
        }

        public async Task<ConfirmationCode> WriteCodeAsync(ConfirmationCode confirmCode)
        {
            await _context.ConfirmationCodes.AddAsync(confirmCode);
            await _context.SaveChangesAsync();
            return confirmCode;
        }
    }
}
