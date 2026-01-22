using System.Globalization;
using System.Reflection.Metadata;

namespace Honalolo.Information.Application.DTOs.Attractions
{
    public class AttractionFilterDto
    {
        public string? SearchQuery { get; set; }
        public string? TypeName { get; set; }
        public string? CityName { get; set; }
        public string? RegionName { get; set; }
        public string? CountryName { get; set; }
        public string? ContinentName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        // ...
    }
}