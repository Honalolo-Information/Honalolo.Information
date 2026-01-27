using Honalolo.Information.Application.DTOs.Reports;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateReportPdf(ReportDto reportData);
    }
}