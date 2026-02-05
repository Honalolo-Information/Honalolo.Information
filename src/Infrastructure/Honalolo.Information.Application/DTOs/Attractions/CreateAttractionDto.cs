using System.ComponentModel.DataAnnotations;

namespace Honalolo.Information.Application.DTOs.Attractions
{
    namespace Honalolo.Information.Application.DTOs.Attractions
    {
        public class CreateAttractionDto
        {
            [Required]
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }

            [Required]
            public string TypeName { get; set; } = string.Empty;

            [Required]
            public string CityName { get; set; } = string.Empty;
            [Required]
            public string RegionName { get; set; } = string.Empty;
            [Required]
            public string CountryName { get; set; } = string.Empty;
            [Required]
            public string ContinentName { get; set; } = string.Empty;

            // Details...
            public CreateEventDto? EventDetails { get; set; }
            public CreateTrailDto? TrailDetails { get; set; }
            public CreateHotelDto? HotelDetails { get; set; }
            public CreateFoodDto? FoodDetails { get; set; }

            // New fields for additional details
            public List<string> OpeningHours { get; set; } = new();
            public List<string> Languages { get; set; } = new();
        }
    }

    public class CreateEventDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateTrailDto
    {
        public int DistanceMeters { get; set; }
        public int DifficultyLevelId { get; set; }
    }

    public class CreateHotelDto
    {
        public string Amenities { get; set; } = string.Empty;
    }

    public class CreateFoodDto
    {
        public string FoodType { get; set; } = string.Empty;
    }
}