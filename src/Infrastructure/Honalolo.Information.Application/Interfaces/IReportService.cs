using Honalolo.Information.Application.DTOs.Reports;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> GenerateReportAsync(GenerateReportRequestDto request, int adminUserId);
        Task<ReportDto?> GetReportByIdAsync(int id);
    }
}