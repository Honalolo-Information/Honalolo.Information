using System.ComponentModel.DataAnnotations;

namespace Honalolo.Information.Application.DTOs.Attractions
{
    public class CreateAttractionDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CityId { get; set; }
        public int TypeId { get; set; } // 1=Event, 2=Trail, etc.
        public decimal Price { get; set; }

        // Specific details (Optional - Frontend sends only what's needed)
        public CreateEventDto? EventDetails { get; set; }
        public CreateTrailDto? TrailDetails { get; set; }
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
}