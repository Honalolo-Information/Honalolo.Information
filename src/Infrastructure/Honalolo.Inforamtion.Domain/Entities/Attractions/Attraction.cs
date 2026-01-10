using Honalolo.Inforamtion.Domain.Common;
using Honalolo.Inforamtion.Domain.Entities.Locations;

namespace Honalolo.Inforamtion.Domain.Entities.Attractions
{
    public class Attraction : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LocationDetails { get; set; } = string.Empty; // Maps to TEXT location
        public decimal Price { get; set; }
        public string ImagesJson { get; set; } = "[]"; // Maps to JSON images

        // Relationships
        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;

        public int CityId { get; set; }
        public City City { get; set; } = null!;

        public int TypeId { get; set; }
        public AttractionType Type { get; set; } = null!;

        // Extension Tables (One-to-One or One-to-Many depending on logic)
        // Using nullable because an Attraction might NOT be an event or a trail.
        public Event? EventDetails { get; set; }
        public Trail? TrailDetails { get; set; }
        public Hotel? HotelDetails { get; set; }
        public Food? FoodDetails { get; set; }

        // Multi-value tables (One-to-Many)
        public ICollection<AttractionLanguage> Languages { get; set; } = new List<AttractionLanguage>();
        public ICollection<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();
    }
}
