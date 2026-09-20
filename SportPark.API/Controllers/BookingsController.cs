using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPark.BusinessLogic;
using SportPark.Domains.Entities;
using SportPark.Domains.Models;
using System.Security.Claims;

namespace SportPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // все эндпоинты этого контроллера требуют входа в систему
    public class BookingsController : ControllerBase
    {
        private readonly BookingManagementService _bookingManagementService;

        public BookingsController(BookingManagementService bookingManagementService)
        {
            _bookingManagementService = bookingManagementService;
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(idClaim);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingCreateRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var created = await _bookingManagementService.Create(userId, request);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = GetCurrentUserId();
            return Ok(await _bookingManagementService.GetMyBookings(userId));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetCurrentUserId();
            var success = await _bookingManagementService.Cancel(userId, id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Trainer,Admin")]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");
            var success = await _bookingManagementService.MarkCompleted(userId, isAdmin, id);
            if (!success) return BadRequest(new { message = "Невозможно списать это занятие (не найдено, не сегодняшнее, или не ваше)" });
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookingManagementService.GetAll());
        }
    }
}
