using System.Collections.Generic;

namespace Honalolo.Information.Application.DTOs.Reports
{
    public enum ReportType
    {
        Top5Expensive = 0,
        DetailedList = 1
    }

    public class GenerateReportRequestDto
    {
        public ReportType Type { get; set; } = ReportType.Top5Expensive; // Default to original behavior
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? CityName { get; set; }
    }

    public class AttractionSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public DateTime? EventDate { get; set; }
    }

    public class ReportStatsDto
    {
        public int TotalAttractions { get; set; }
        public decimal AveragePrice { get; set; }
        public Dictionary<string, int> CountByType { get; set; } = new();
        public List<AttractionSummaryDto> MostExpensiveAttractions { get; set; } = new();
        public List<AttractionSummaryDto> AllMatchingAttractions { get; set; } = new();
    }

    public class ReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime GeneratedAt { get; set; }
        public ReportStatsDto Stats { get; set; }
        public GenerateReportRequestDto Parameters { get; set; }
    }

    public class ReportListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime GeneratedAt { get; set; }
        public GenerateReportRequestDto Parameters { get; set; }
    }
}