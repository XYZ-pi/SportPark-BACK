using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPark.BusinessLogic;
using SportPark.Domains.Enums;

namespace SportPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManagementService _userManagementService;

        public UsersController(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserRole? role)
        {
            var isAdmin = User.IsInRole("Admin");
            var isTrainer = User.IsInRole("Trainer");

            // Тренеру разрешаем видеть только список клиентов (нужно для создания индивидуальной тренировки)
            if (!isAdmin && (!isTrainer || role != UserRole.Client))
                return Forbid();

            return Ok(await _userManagementService.GetByRole(role));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userManagementService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}