using Testing.Domain.DTOs.Sessions;
using Testing.Domain.DTOs.Users;

namespace Testing.Services.Interfaces
{
    /// <summary>
    /// Сервис для управления аутентификацией и регистрацией пользователей
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Регистрирует нового пользователя и отправляет код подтверждения на email
        /// </summary>
        /// <param name="userCreateDTO">Данные нового пользователя (имя, фамилия, email, пароль)</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns></returns>
        Task RegisterUserAsync(UserCreateDTO userCreateDTO, CancellationToken token = default);

        /// <summary>
        /// Выполняет вход пользователя и возвращает токены доступа
        /// </summary>
        /// <param name="authDTO">Данные для входа (email, пароль)</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Токены доступа (Access и Refresh)</returns>
        Task<TokenDTO> AuthUserAsync(AuthDTO authDTO, CancellationToken token = default);

        /// <summary>
        /// Обновляет пару токенов с использованием Refresh токена
        /// </summary>
        /// <param name="refreshToken">Refresh токен из предыдущей сессии</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns>Новая пара токенов (Access и Refresh)</returns>
        Task<TokenDTO> RefreshAsync(string refreshToken, CancellationToken token = default);

        /// <summary>
        /// Выполняет выход пользователя (деактивирует Refresh токен)
        /// </summary>
        /// <param name="refreshToken">Refresh токен для деактивации</param>
        /// <param name="token">Токен отмены операции</param>
        /// <returns></returns>
        Task LogoutAsync(string refreshToken, CancellationToken token = default);

        /// <summary>
        /// Отправляет 6-значный код подтверждения на указанный email
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <returns></returns>
        Task SendRegisterEmailCodeAsync(string email);

        /// <summary>
        /// Подтверждает email пользователя с помощью кода и завершает регистрацию
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="code">6-значный код из письма</param>
        /// <returns>Токены доступа для автоматического входа</returns>
        Task<TokenDTO> ConfirmEmailAsync(string email, string code);

        Task<List<CountryDTO>> GetCountriesAsync();
    }
}
