using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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

        [HttpPut("{id}/photos")]
        [Authorize(Roles = "Administrator,Moderator,Partner")]
        public async Task<ActionResult<AttractionDetailDto>> OverwritePhotos(int id, [FromForm] List<IFormFile> files)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            try
            {
                // Logic is: Delete old ones -> Save new ones
                var result = await _service.UpdatePhotosAsync(id, files, int.Parse(userIdString));
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Moderator,Partner,RegisteredUser")]
        public async Task<ActionResult<AttractionDetailDto>> Update(int id, [FromBody] UpdateAttractionDto dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            int userId = int.Parse(userIdString);
            bool isAdmin = userRole == "Administrator";

            try
            {
                var result = await _service.UpdateAsync(id, dto, userId, isAdmin);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
             // Although Authorize attribute handles role check, we pass flags to service if needed for consistency or logging
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
             int userId = int.Parse(userIdString);
             
             // Since [Authorize(Roles = "Administrator")] is already there, we know it's an admin
             bool isAdmin = true;

            try
            {
                var result = await _service.DeleteAsync(id, userId, isAdmin);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                 // Should be caught by the attribute, but double safety
                return Forbid();
            }
        }
    }
}