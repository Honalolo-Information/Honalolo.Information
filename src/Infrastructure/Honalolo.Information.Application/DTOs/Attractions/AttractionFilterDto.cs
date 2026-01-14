namespace Honalolo.Information.Application.DTOs.Attractions
{
    public class AttractionFilterDto
    {
        public int? TypeId { get; set; }
        public int? CityId { get; set; }
        public int? RegionId { get; set; }
        public int? CountryId { get; set; }
        public int? ContinentId { get; set; }
        // Optional: Filter by specific details
        public DateTime? EventDateFrom { get; set; }
        public DateTime? EventDateTo { get; set; }
    }
}