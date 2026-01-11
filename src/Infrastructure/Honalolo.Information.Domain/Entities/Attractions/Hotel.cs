using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Attractions
{
    public class Hotel : BaseEntity
    {
        public int AttractionId { get; set; } 
        public Attraction Attraction { get; set; } = null!;
        public string AmenitiesJson { get; set; } = "[]"; // Maps to JSON amenities
    }
}
