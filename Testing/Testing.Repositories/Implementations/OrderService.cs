using AutoMapper;
using Testing.Domain.DTOs.Orders;
using Testing.Repositories.Interfaces;

namespace Testing.Repositories.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateOrderAsync(int userId, CreateOrderDTO dto)
        {
            var product = await _productRepository.GetByIdWithSpecAsync(dto.IdProduct);
            if (product == null)
                throw new Exception("Товар не найден");

            var totalPrice = (product.Price ?? 0) * dto.Quantity;

            var order = new Testing.Domain.Entities.Order
            {
                IdUser = userId,
                IdProduct = dto.IdProduct,
                Quantity = dto.Quantity,
                TotalPrice = totalPrice,
                Status = "pending"
            };

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            var orderDto = _mapper.Map<OrderDTO>(createdOrder);

            orderDto.ProductName = product.Name;
            orderDto.StatusText = GetStatusText(createdOrder.Status);

            return orderDto;
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            var orderDtos = _mapper.Map<List<OrderDTO>>(orders);

            foreach (var orderDto in orderDtos)
            {
                var product = await _productRepository.GetByIdWithSpecAsync(orderDto.IdProduct);
                orderDto.ProductName = product?.Name;
                orderDto.StatusText = GetStatusText(orderDto.Status);
            }

            return orderDtos;
        }

        private string GetStatusText(string? status)
        {
            return status switch
            {
                "pending" => "Ожидает оплаты",
                "processing" => "В производстве",
                "shipped" => "Отправлен",
                "delivered" => "Доставлен",
                "cancelled" => "Отменён",
                _ => status ?? "Неизвестно"
            };
        }
    }
}
