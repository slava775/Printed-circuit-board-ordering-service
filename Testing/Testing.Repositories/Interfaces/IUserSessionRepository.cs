using Testing.Domain.Entities;

namespace Testing.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с сессиями пользователей (Refresh токенами)
    /// </summary>
    public interface IUserSessionRepository
    {
        /// <summary>
        /// Получить сессию по Refresh токену
        /// </summary>
        /// <param name="refreshToken">Refresh токен</param>
        /// <returns>Сессия пользователя или null, если не найдена</returns>
        Task<UserSession?> GetByRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Получить только активные сессии
        /// </summary>
        /// <param name="refreshToken">Refresh токен</param>
        /// <returns>Активная сессия или null, если не найдена или истекла</returns>
        Task<UserSession?> GetActiveByRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Создать новую сессию для пользователя
        /// </summary>
        /// <param name="session">Объект сессии (содержит Id пользователя, Refresh токен, даты)</param>
        /// <returns></returns>
        Task CreateSessionAsync(UserSession session);

        /// <summary>
        /// Деактивировать сессию по Refresh токену
        /// </summary>
        /// <param name="refreshToken">Refresh токен сессии для деактивации</param>
        /// <returns></returns>
        Task DeactivateSessionAsync(string refreshToken);

        /// <summary>
        /// Деактивировать все активные сессии пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task DeactivateAllUserSessionsAsync(int userId);
    }
}
