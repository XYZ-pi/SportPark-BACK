using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPark.BusinessLogic;
using SportPark.Domains.Models;

namespace SportPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.Login(request.Email, request.Password);

            if (token == null)
                return Unauthorized(new { message = "Неверный email или пароль" });

            return Ok(new { token });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")] // только админ может создавать пользователей
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterUser(
                    request.Name, request.Email, request.Password, request.Phone, request.Role);

                return Ok(new { user.Id, user.Name, user.Email, Role = user.Role.ToString() });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}