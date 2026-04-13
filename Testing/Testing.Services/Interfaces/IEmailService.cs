namespace Testing.Services.Interfaces
{
    /// <summary>
    /// Сервис для отправки электронных писем через SMTP
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Асинхронно отправляет электронное письмо на указанный адрес
        /// </summary>
        /// <param name="toEmail">Email получателя</param>
        /// <param name="subject">Тема письма</param>
        /// <param name="body">Содержимое письма в формате HTML</param>
        /// <returns></returns>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
