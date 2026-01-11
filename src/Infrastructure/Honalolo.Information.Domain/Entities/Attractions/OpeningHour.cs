using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Attractions
{
    public class OpeningHour : BaseEntity
    {
        // The actual text from the database (e.g., "Monday: 09:00 - 17:00")
        public string Content { get; set; } = string.Empty;

        // Foreign Key Relationship
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; } = null!;
    }
}
