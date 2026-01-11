namespace Honalolo.Information.Application.DTOs.Attractions
{
    public class AttractionDetailDto : AttractionDto
    {
        public string Description { get; set; } = string.Empty;
        public string LocationDetails { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new();
        public List<string> Languages { get; set; } = new();
        public List<string> OpeningHours { get; set; } = new();

        // Conditional Details (Null if not applicable)
        public EventDto? EventDetails { get; set; }
        public TrailDto? TrailDetails { get; set; }
        public HotelDto? HotelDetails { get; set; }
        public FoodDto? FoodDetails { get; set; }
    }

    public class EventDto
    {
        public string EventType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class TrailDto
    {
        public int DistanceMeters { get; set; }
        public string Difficulty { get; set; } = string.Empty; // "Hard" instead of "2"
    }

    public class HotelDto
    {
        public string Amenities { get; set; } = string.Empty;
    }

    public class FoodDto
    {
        public string FoodType { get; set; } = string.Empty;
    }
}
