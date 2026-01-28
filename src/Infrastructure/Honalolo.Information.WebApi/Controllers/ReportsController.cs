using Honalolo.Information.Application.DTOs.Reports;
using Honalolo.Information.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Honalolo.Information.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    [EnableCors("AllowWebApp")]
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

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadReportPdf(int id)
        {
            var pdfBytes = await _reportService.GetReportPdfAsync(id);

            if (pdfBytes == null)
                return NotFound("Report not found.");

            string fileName = $"report_{id}_{DateTime.Now:yyyyMMdd}.pdf";

            // Zwracamy plik. "application/pdf" mówi przeglądarce, co to za typ.
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}