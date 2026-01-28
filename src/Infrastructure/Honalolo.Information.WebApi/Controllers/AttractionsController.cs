using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Honalolo.Information.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowWebApp")]
    public class AttractionsController : ControllerBase
    {
        private readonly IAttractionService _service;

        public AttractionsController(IAttractionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator,Partner,RegisteredUser")]
        public async Task<ActionResult<IEnumerable<AttractionDto>>> GetAll([FromQuery] AttractionFilterDto filter)
        {
            try
            {
                // If filter is empty, it returns everything. If populated, it filters.
                var result = await _service.SearchAsync(filter);
                return Ok(result);
            }
            catch { return BadRequest("Invalid filter parameters."); }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Moderator,Partner,RegisteredUser")]
        public async Task<ActionResult<AttractionDetailDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

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

        [HttpPost("{id}/photos")]
        [Authorize(Roles = "Administrator,Moderator,Partner")]
        public async Task<ActionResult<AttractionDetailDto>> UploadPhotos(int id, [FromForm] List<IFormFile> files)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            try
            {
                var result = await _service.AddPhotosAsync(id, files, int.Parse(userIdString));
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}