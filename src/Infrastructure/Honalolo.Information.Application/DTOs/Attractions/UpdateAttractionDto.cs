using System.ComponentModel.DataAnnotations;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;

namespace Honalolo.Information.Application.DTOs.Attractions
{
    public class UpdateAttractionDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // We allow updating the location partially or fully
        public string? CityName { get; set; }
        public string? RegionName { get; set; }
        public string? CountryName { get; set; }
        public string? ContinentName { get; set; }

        // Details - optional, if provided they will update existing details
        public CreateEventDto? EventDetails { get; set; }
        public CreateTrailDto? TrailDetails { get; set; }
        public CreateHotelDto? HotelDetails { get; set; }
        public CreateFoodDto? FoodDetails { get; set; }

        public List<string>? OpeningHours { get; set; }
        public List<string>? Languages { get; set; }
    }
}
