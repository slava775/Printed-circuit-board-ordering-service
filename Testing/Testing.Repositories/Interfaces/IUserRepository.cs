using Testing.Domain.Entities;

namespace Testing.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с пользователями в базе данных
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получить пользователя по его идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор пользователя</param>
        /// <returns>Объект пользователя или null, если пользователь не найден</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Получить пользователя по его email адресу
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <returns>Объект пользователя или null, если пользователь не найден</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Проверить, существует ли пользователь с указанным email
        /// </summary>
        /// <param name="email">Email для проверки</param>
        /// <returns>true - если email уже занят, false - если свободен</returns>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Создать нового пользователя в базе данных
        /// </summary>
        /// <param name="user">Объект пользователя для создания</param>
        /// <returns></returns>
        Task CreateAsync(User user);

        /// <summary>
        /// Обновить данные существующего пользователя
        /// </summary>
        /// <param name="user">Объект пользователя с обновленными данными</param>
        /// <returns></returns>
        Task UpdateAsync(User user);

        /// <summary>
        /// Получить список всех стран
        /// </summary>
        Task<List<Country>> GetCountriesAsync();
    }
}
