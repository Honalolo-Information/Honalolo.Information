using Honalolo.Information.Application.DTOs.Reports;
using Honalolo.Information.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Honalolo.Information.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateReportPdf(ReportDto report)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // 1. HEADER
                    page.Header()
                        .Text($"Honalolo Admin Report: {report.Title}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    // 2. CONTENT
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Item().Text($"Generated At: {report.GeneratedAt:yyyy-MM-dd HH:mm}");
                            x.Item().Text($"Total Attractions: {report.Stats.TotalAttractions}");
                            x.Item().Text($"Average Price: {report.Stats.AveragePrice} PLN");

                            x.Item().PaddingTop(10).LineHorizontal(1).LineColor(Color.FromRGB(255,255,255));

                            // Tabela: Typy Atrakcji
                            x.Item().PaddingTop(10).Text("Attractions by Type").Bold().FontSize(14);
                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Type");
                                    header.Cell().Element(CellStyle).Text("Count");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                foreach (var item in report.Stats.CountByType)
                                {
                                    table.Cell().Element(CellStyle).Text(item.Key);
                                    table.Cell().Element(CellStyle).Text(item.Value.ToString());

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                    }
                                }
                            });

                            x.Item().PaddingTop(20).Text("Top 5 Most Expensive").Bold().FontSize(14);
                            foreach (var expensive in report.Stats.MostExpensiveAttractions)
                            {
                                x.Item().Text($"• {expensive}");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}