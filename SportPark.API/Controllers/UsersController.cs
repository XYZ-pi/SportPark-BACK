using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPark.BusinessLogic;
using SportPark.Domains.Enums;

namespace SportPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
            return Ok(await _userManagementService.GetByRole(role));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userManagementService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}