using Microsoft.AspNetCore.Mvc;
using Testing.Domain.DTOs.Sessions;
using Testing.Domain.DTOs.Users;
using Testing.Services.Interfaces;

namespace Testing.API.Controllers
{
    /// <summary>
    /// Контроллер для управления аутентификацией пользователей
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("registr")]
        public async Task<ActionResult<TokenDTO>> Registr(UserCreateDTO userCreateDTO)
        {
            await _authService.RegisterUserAsync(userCreateDTO);
            return Ok(new { message = "Регистрация успешна. Код подтверждения отправлен на email" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(AuthDTO authDTO)
        {
            var result = await _authService.AuthUserAsync(authDTO);
            return Ok(result);
        }

        // POST: api/auth/refresh
        [HttpPost("refresh")]
        public async Task<ActionResult<TokenDTO>> Refresh([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshAsync(refreshToken);
            return Ok(result);
        }

        // GET: api/auth/countries
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _authService.GetCountriesAsync();
            return Ok(countries);
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _authService.LogoutAsync(refreshToken);
            return Ok(new { message = "Выход выполнен успешно" });
        }

        /// <summary>
        /// Отправка кода подтверждения на email
        /// </summary>
        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode([FromBody] string email)
        {
            try
            {
                await _authService.SendRegisterEmailCodeAsync(email);
                return Ok(new { message = "Код подтверждения отправлен на email" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        /// <summary>
        /// Подтверждение email с помощью кода
        /// </summary>
        [HttpPost("confirm-email")]
        public async Task<ActionResult<TokenDTO>> ConfirmEmail(string email, string code)
        {
            try
            {
                var result = await _authService.ConfirmEmailAsync(email, code);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
