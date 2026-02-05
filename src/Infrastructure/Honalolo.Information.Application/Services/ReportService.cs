using System.Text.Json;
using Honalolo.Information.Application.DTOs.Reports;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Domain.Entities.Reports;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Honalolo.Information.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly TouristInfoDbContext _context;
        private readonly IPdfService _pdfService;

        public ReportService(TouristInfoDbContext context, IPdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        public async Task<ReportDto> GenerateReportAsync(GenerateReportRequestDto request, int adminUserId)
        {
            // 1. Start building query
            var query = _context.Attractions
                .Include(a => a.Type)
                .Include(a => a.EventDetails)
                .Include(a => a.City)
                .AsQueryable();

            // 2. Apply Filters
            if (request.MinPrice.HasValue)
                query = query.Where(a => a.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(a => a.Price <= request.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(request.CityName))
                query = query.Where(a => a.City.Name == request.CityName);

            if (request.StartDate.HasValue || request.EndDate.HasValue)
            {
                // Filter specifically for Events within date range
                query = query.Where(a => a.EventDetails != null &&
                                         (!request.StartDate.HasValue || a.EventDetails.StartDate >= request.StartDate) &&
                                         (!request.EndDate.HasValue || a.EventDetails.EndDate <= request.EndDate));
            }

            var attractions = await query.ToListAsync();

            // 3. Calculate Stats
            var stats = new ReportStatsDto
            {
                TotalAttractions = attractions.Count,
                AveragePrice = attractions.Any() ? Math.Round(attractions.Average(a => a.Price), 2) : 0,
                CountByType = attractions
                    .GroupBy(a => a.Type.TypeName)
                    .ToDictionary(g => g.Key, g => g.Count()),
                MostExpensiveAttractions = attractions
                    .OrderByDescending(a => a.Price)
                    .Take(5)
                    .Select(a => new AttractionSummaryDto
                    {
                        Title = a.Title,
                        Price = a.Price,
                        EventDate = a.EventDetails != null ? a.EventDetails.StartDate : null
                    })
                    .ToList()
            };

            // 4. Save Report to DB
            var reportEntity = new Report
            {
                Title = $"Report {DateTime.UtcNow:yyyy-MM-dd HH:mm}",
                GeneratedAt = DateTime.UtcNow,
                RequestedByUserId = adminUserId,
                ParametersJson = JsonSerializer.Serialize(request),
                DataJson = JsonSerializer.Serialize(stats)
            };

            _context.Reports.Add(reportEntity);
            await _context.SaveChangesAsync();

            // 5. Return DTO
            return new ReportDto
            {
                Id = reportEntity.Id,
                Title = reportEntity.Title,
                GeneratedAt = reportEntity.GeneratedAt,
                Stats = stats,
                Parameters = request
            };
        }

        public async Task<ReportDto?> GetReportByIdAsync(int id)
        {
            var entity = await _context.Reports.FindAsync(id);
            if (entity == null) return null;

            return new ReportDto
            {
                Id = entity.Id,
                Title = entity.Title,
                GeneratedAt = entity.GeneratedAt,
                Stats = JsonSerializer.Deserialize<ReportStatsDto>(entity.DataJson) ?? new ReportStatsDto(),
                Parameters = JsonSerializer.Deserialize<GenerateReportRequestDto>(entity.ParametersJson)
            };
        }

        public async Task<byte[]?> GetReportPdfAsync(int id)
        {
            // 1. Pobierz dane raportu (używamy istniejącej logiki)
            var reportDto = await GetReportByIdAsync(id);

            if (reportDto == null) return null;

            // 2. Wygeneruj PDF
            return _pdfService.GenerateReportPdf(reportDto);
        }
    }
}