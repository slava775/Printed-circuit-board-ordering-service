using Microsoft.EntityFrameworkCore;
using Testing.Domain.Entities;
using Testing.Repositories.Interfaces;

namespace Testing.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbPrintedBoardsContext _context;

        public OrderRepository(DbPrintedBoardsContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.OrderNumber = $"PCB-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
            order.CreatedAt = DateTime.UtcNow;
            order.Status = "pending";

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
            .Include(o => o.IdProductNavigation)
            .FirstOrDefaultAsync(o => o.IdOrder == id);
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.IdProductNavigation)
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
