using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPark.BusinessLogic;
using SportPark.Domains.Models;
using System.Security.Claims;

namespace SportPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalSessionsController : ControllerBase
    {
        private readonly PersonalSessionManagementService _personalSessionManagementService;

        public PersonalSessionsController(PersonalSessionManagementService personalSessionManagementService)
        {
            _personalSessionManagementService = personalSessionManagementService;
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> Create([FromBody] PersonalSessionCreateRequest request)
        {
            try
            {
                var trainerUserId = GetCurrentUserId();
                var created = await _personalSessionManagementService.Create(trainerUserId, request);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-as-trainer")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetMyAsTrainer()
        {
            var userId = GetCurrentUserId();
            return Ok(await _personalSessionManagementService.GetForTrainer(userId));
        }

        [HttpGet("my-as-client")]
        public async Task<IActionResult> GetMyAsClient()
        {
            var userId = GetCurrentUserId();
            return Ok(await _personalSessionManagementService.GetForClient(userId));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetCurrentUserId();
            var success = await _personalSessionManagementService.Cancel(userId, id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            var success = await _personalSessionManagementService.MarkCompleted(userId, isAdmin, id);
            if (!success) return BadRequest(new { message = "Невозможно списать эту тренировку" });
            return NoContent();
        }
    }
}