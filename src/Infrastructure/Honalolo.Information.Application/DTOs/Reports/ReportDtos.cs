using System.Collections.Generic;

namespace Honalolo.Information.Application.DTOs.Reports
{
    public class GenerateReportRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? CityName { get; set; }
    }

    public class AttractionSummaryDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime? EventDate { get; set; }
    }

    public class ReportStatsDto
    {
        public int TotalAttractions { get; set; }
        public decimal AveragePrice { get; set; }
        public Dictionary<string, int> CountByType { get; set; } = new();
        public List<AttractionSummaryDto> MostExpensiveAttractions { get; set; } = new();
    }

    public class ReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime GeneratedAt { get; set; }
        public ReportStatsDto Stats { get; set; }
        public GenerateReportRequestDto Parameters { get; set; }
    }
}