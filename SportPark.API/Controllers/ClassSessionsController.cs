using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPark.BusinessLogic;
using SportPark.Domains.Models;

namespace SportPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSessionsController : ControllerBase
    {
        private readonly ClassSessionManagementService _classSessionManagementService;

        public ClassSessionsController(ClassSessionManagementService classSessionManagementService)
        {
            _classSessionManagementService = classSessionManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _classSessionManagementService.GetAll());
        }

        [HttpGet("day/{day}")]
        public async Task<IActionResult> GetByDay(DayOfWeek day)
        {
            return Ok(await _classSessionManagementService.GetByDay(day));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ClassSessionCreateRequest request)
        {
            try
            {
                var created = await _classSessionManagementService.Create(request);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _classSessionManagementService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}