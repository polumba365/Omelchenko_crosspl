using Microsoft.AspNetCore.Mvc;
using Omelchenko_crosspl.Models;

namespace Omelchenko_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Вспомогательный класс для приема данных логина
        public class LoginRequest
        {
            public string Login { get; set; } = "";
            public string Password { get; set; } = "";
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Простая проверка логина и пароля (в реальном проекте — проверка по БД)
            bool isValid = (request.Login == "admin" && request.Password == "admin123") ||
                           (request.Login == "user" && request.Password == "user123");

            if (!isValid)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            // Определяем роль: если логин "admin" — то админ, иначе пользователь
            bool isAdmin = request.Login == "admin";

            // Генерируем токен через AuthOptions
            var token = AuthOptions.GenerateToken(isAdmin);

            return Ok(token);
        }
    }
}