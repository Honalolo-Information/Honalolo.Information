using Microsoft.AspNetCore.Mvc;
using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.Interfaces;

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
        public async Task<ActionResult<IEnumerable<AttractionDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
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
        public async Task<ActionResult<int>> Create([FromBody] CreateAttractionDto dto, [FromHeader(Name = "X-User-Id")] int userId)
        {
            // TEMPORARY: For testing without Auth, we read UserID from a header.
            // Later, we will get this from the JWT Token (User.Identity).
            if (userId == 0) userId = 1; // Default to Admin for testing if header missing

            var newId = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }

        // GET: api/attractions/events?regionId=1
        [HttpGet("events")]
        public async Task<ActionResult<IEnumerable<AttractionDto>>> GetEvents([FromQuery] int regionId)
        {
            var result = await _service.GetEventsByRegionAsync(regionId);
            return Ok(result);
        }
    }
}