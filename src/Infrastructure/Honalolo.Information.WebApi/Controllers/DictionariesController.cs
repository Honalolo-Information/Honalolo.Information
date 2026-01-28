using Honalolo.Information.Application.DTOs.General;
using Honalolo.Information.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Honalolo.Information.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowWebApp")]
    public class DictionariesController : ControllerBase
    {
        private readonly IDictionaryService _service;

        public DictionariesController(IDictionaryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<DictionaryDto>> GetOptions()
        {
            var result = await _service.GetAllOptionsAsync();
            return Ok(result);
        }
    }
}