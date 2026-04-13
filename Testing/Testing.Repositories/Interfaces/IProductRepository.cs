using Testing.Domain.Entities;

namespace Testing.Repositories.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Получить все активные товары
        /// </summary>
        Task<List<Product>> GetAllActiveAsync();

        /// <summary>
        /// Получить товар по ID с характеристиками
        /// </summary>
        Task<Product> GetByIdWithSpecAsync(int id);

        /// <summary>
        /// Получить товары по категории
        /// </summary>
        Task<List<Product>> GetByCategoryAsync(int categoryId);

        /// <summary>
        /// Получить популярные товары
        /// </summary>
        Task<List<Product>> GetPopularAsync(int count);
    }
}
