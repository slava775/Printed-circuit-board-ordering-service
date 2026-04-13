using Testing.Domain.Entities;

namespace Testing.Repositories.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с кодами подтверждения email
    /// </summary>
    public interface IMailRepository
    {
        /// <summary>
        /// Сохраняет новый код подтверждения в базу данных
        /// </summary>
        /// <param name="confirmCode">Объект кода подтверждения (email, код, срок действия)</param>
        /// <returns>Сохраненный код с присвоенным ID</returns>
        Task<ConfirmationCode> WriteCodeAsync(ConfirmationCode confirmCode);

        /// <summary>
        /// Проверяет существует ли код для указанного email и не истек ли он
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="code">6-значный код подтверждения</param>
        /// <returns>Код подтверждения если найден и действителен, иначе null</returns>
        Task<ConfirmationCode?> VerifyCodeAsync(string email, string code);

        /// <summary>
        /// Удаляет код подтверждения из базы данных
        /// </summary>
        /// <param name="confirmCode">Объект кода для удаления</param>
        /// <returns></returns>
        Task DeleteCodeAsync(ConfirmationCode confirmCode);

        /// <summary>
        /// Находит последний активный код подтверждения по email
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <returns>Последний код подтверждения или null</returns>
        Task<ConfirmationCode?> FindByEmailAsync(string email);
    }
}
