namespace Honalolo.Information.Application.DTOs.Attractions
{
    public class AttractionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty; // Flattened from City object
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public int ContinentId { get; set; }
        public string ContinentName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty; // Flattened from Type object
        public decimal Price { get; set; }
        public string MainImage { get; set; } = string.Empty; // Extracted from JSON
        public int AuthorId { get; set; }
        public List<string> OpeningHours { get; set; } = new();

        // Specific properties flattened for list view if needed, or kept structural
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? TrailDifficulty { get; set; }
        public string? FoodType { get; set; }
        public string? HotelAmenities { get; set; }
    }
}
