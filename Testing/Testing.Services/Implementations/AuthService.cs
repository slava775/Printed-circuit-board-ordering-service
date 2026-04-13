using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Testing.Domain.DTOs.Sessions;
using Testing.Domain.DTOs.Users;
using Testing.Domain.Entities;
using Testing.Repositories.Interfaces;
using Testing.Services.Interfaces;

namespace Testing.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSessionRepository _sessionRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailRepository _mailRepository;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository userRepository, IUserSessionRepository sessionRepository, IConfiguration configuration, IMailRepository mailRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _configuration = configuration;
            _mailRepository = mailRepository;
            _emailService = emailService;
        }

        public async Task<TokenDTO> AuthUserAsync(AuthDTO authDTO, CancellationToken token = default)
        {
            var user = await _userRepository.GetByEmailAsync(authDTO.Email);

            if (user == null || user.PasswordHash != HashPassword(authDTO.Password))
            {
                throw new UnauthorizedAccessException("Неверный email или пароль");
            }

            return await GenerateTokenDTOAsync(user, token);
        }

        public async Task LogoutAsync(string refreshToken, CancellationToken token = default)
        {
            await _sessionRepository.DeactivateSessionAsync(refreshToken);
        }

        public async Task<TokenDTO> RefreshAsync(string refreshToken, CancellationToken token = default)
        {
            var session = await _sessionRepository.GetActiveByRefreshTokenAsync(refreshToken);

            if (session == null)
            {
                throw new UnauthorizedAccessException("Недействительный или истекший токен");
            }

            if (!session.IdUser.HasValue)
            {
                throw new UnauthorizedAccessException("ID пользователя не найден в сессии");
            }

            var user = await _userRepository.GetByIdAsync(session.IdUser.Value);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Пользователь не найден");
            }

            await _sessionRepository.DeactivateSessionAsync(refreshToken);

            return await GenerateTokenDTOAsync(user, token);
        }

        public async Task RegisterUserAsync(UserCreateDTO userCreateDTO, CancellationToken token = default)
        {
            var emailExists = await _userRepository.EmailExistsAsync(userCreateDTO.Email);

            if (emailExists)
            {
                throw new InvalidOperationException("Пользователь с таким email уже существует");
            }

            var user = new User
            {
                Name = userCreateDTO.Name,
                Surname = userCreateDTO.Surname,
                Email = userCreateDTO.Email,
                Password = HashPassword(userCreateDTO.Password),
                PasswordHash = HashPassword(userCreateDTO.Password),
                IdRole = 2,
                EmailConfirmed = false,
                IdCountry = userCreateDTO.IdCountry 
            };

            await _userRepository.CreateAsync(user);
            await SendRegisterEmailCodeAsync(userCreateDTO.Email);
        }

        public async Task<List<CountryDTO>> GetCountriesAsync()
        {
            var countries = await _userRepository.GetCountriesAsync();

            return countries.Select(c => new CountryDTO
            {
                IdCountry = c.IdCountry,
                Name = c.Name ?? string.Empty
            }).ToList();
        }

        public async Task SendRegisterEmailCodeAsync(string email)
        {
            var code = GenerateCode();

            var confirmationCode = new ConfirmationCode
            {
                Email = email,
                Code = code,
                ExpiresIn = DateTime.UtcNow.AddMinutes(15),
            };

            await _mailRepository.WriteCodeAsync(confirmationCode);

            await _emailService.SendEmailAsync(email, "Код подтверждения регистрации", $"Ваш код подтверждения: <strong>{code}</strong><br>Код действителен 15 минут.");
        }

        public async Task<TokenDTO> ConfirmEmailAsync(string email, string code)
        {
            var validCode = await _mailRepository.VerifyCodeAsync(email, code);

            if (validCode == null)
            {
                throw new UnauthorizedAccessException("Неверный или истекший код подтверждения");
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            user.EmailConfirmed = true;
            await _userRepository.UpdateAsync(user);
            await _mailRepository.DeleteCodeAsync(validCode);
            return await GenerateTokenDTOAsync(user, CancellationToken.None);
        }

        private async Task<TokenDTO> GenerateTokenDTOAsync(User user, CancellationToken token)
        {
            var refreshToken = GenerateRefreshToken();

            var session = new UserSession
            {
                IdUser = user.IdUser,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7), 
                IsActive = true
            };

            await _sessionRepository.CreateSessionAsync(session);

            var role = user.IdRoleNavigation?.Name ?? "User";

            return new TokenDTO
            {
                AccessToken = GenerateAccessToken(user, role),
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                TokenType = "Bearer",
                UserId = user.IdUser,
                Email = user.Email,
                Name = $"{user.Name} {user.Surname}",
                Role = role
            };
        }

        private string GenerateAccessToken(User user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateCode()
        {
            Random random = new Random();
            int number = random.Next(1, 999999);  
            return number.ToString("D6");  
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
