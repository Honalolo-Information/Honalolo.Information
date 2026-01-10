using Honalolo.Inforamtion.Domain.Common;
using Honalolo.Inforamtion.Domain.Enums;

namespace Honalolo.Inforamtion.Domain.Entities.Attractions
{
    public class Trail : BaseEntity
    {
        public int AttractionId { get; set; } // FK
        public Attraction Attraction { get; set; } = null!;

        public int DistanceMeters { get; set; }
        public int AltitudeMeters { get; set; }
        public string StartingPoint { get; set; } = string.Empty;
        public string EndpointPoint { get; set; } = string.Empty;

        // Difficulty Level Dictionary
        public int DifficultyLevelId { get; set; }
        public DifficultyLevel? DifficultyLevel { get; set; } = null!;
    }
}
