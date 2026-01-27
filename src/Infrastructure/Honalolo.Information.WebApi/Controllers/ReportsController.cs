using System.Security.Claims;
using Honalolo.Information.Application.DTOs.Reports;
using Honalolo.Information.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Honalolo.Information.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Tylko Administrator ma dostęp do raportów
    [Authorize(Roles = "Administrator")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ReportDto>> GenerateReport([FromBody] GenerateReportRequestDto request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            int userId = int.Parse(userIdString);

            var report = await _reportService.GenerateReportAsync(request, userId);
            return Ok(report);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReport(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }
    }
}