using System.ComponentModel.DataAnnotations;

namespace Testing.Domain.DTOs.Users
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите страну")]
        public int? IdCountry { get; set; } 
    }
}
