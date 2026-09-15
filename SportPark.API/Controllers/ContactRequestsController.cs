using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportPark.BusinessLogic;
using SportPark.Domains.Models;

namespace SportPark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactRequestsController : ControllerBase
    {
        private readonly ContactRequestManagementService _contactRequestManagementService;

        public ContactRequestsController(ContactRequestManagementService contactRequestManagementService)
        {
            _contactRequestManagementService = contactRequestManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactRequestCreateRequest request)
        {
            var created = await _contactRequestManagementService.Create(request);
            return Ok(created);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // только админ видит все заявки
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contactRequestManagementService.GetAll());
        }
    }
}

