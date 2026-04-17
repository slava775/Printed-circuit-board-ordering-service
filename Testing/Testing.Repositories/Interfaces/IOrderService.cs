using Testing.Domain.DTOs.Orders;

namespace Testing.Repositories.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(int userId, CreateOrderDTO dto);
        Task<List<OrderDTO>> GetUserOrdersAsync(int userId);
    }
}
