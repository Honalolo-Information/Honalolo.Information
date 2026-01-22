using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Honalolo.Information.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttractionsController : ControllerBase
    {
        private readonly IAttractionService _service;

        public AttractionsController(IAttractionService service)
        {
            _service = service;
        }

        // GET: api/attractions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttractionDto>>> GetAll([FromQuery] AttractionFilterDto filter)
        {
            // If filter is empty, it returns everything. If populated, it filters.
            var result = await _service.SearchAsync(filter);
            return Ok(result);
        }

        // GET: api/attractions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AttractionDetailDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/attractions
        [HttpPost]
        [Authorize(Roles = "Administrator,Moderator,Partner")]
        public async Task<ActionResult<int>> Create([FromBody] CreateAttractionDto dto)
        {
            // Get User ID safely from the Token
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            int userId = int.Parse(userIdString);

            var newId = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }
    }
}