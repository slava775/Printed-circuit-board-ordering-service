using Testing.Domain.DTOs.Products;

namespace Testing.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с товарами
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Получить все активные товары
        /// </summary>
        Task<List<ProductDTO>> GetAllProductsAsync();

        /// <summary>
        /// Получить товар по ID
        /// </summary>
        Task<ProductDTO?> GetProductByIdAsync(int id);

        /// <summary>
        /// Получить товары по категории
        /// </summary>
        Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId);

        /// <summary>
        /// Получить популярные товары
        /// </summary>
        Task<List<ProductDTO>> GetPopularProductsAsync(int count);
    }
}
