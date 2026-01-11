using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Attractions
{
    public class Food : BaseEntity
    {
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; } = null!;
        public string CuisineType { get; set; } = string.Empty;
    }
}
