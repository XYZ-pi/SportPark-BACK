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
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionManagementService _subscriptionManagementService;

        public SubscriptionsController(SubscriptionManagementService subscriptionManagementService)
        {
            _subscriptionManagementService = subscriptionManagementService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] SubscriptionCreateRequest request)
        {
            try
            {
                var created = await _subscriptionManagementService.Create(request);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _subscriptionManagementService.GetForUser(userId));
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetForUser(int userId)
        {
            return Ok(await _subscriptionManagementService.GetForUser(userId));
        }
    }
}