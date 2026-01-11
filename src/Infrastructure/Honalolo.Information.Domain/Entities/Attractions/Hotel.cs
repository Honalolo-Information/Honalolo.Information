using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Attractions
{
    public class Hotel : BaseEntity
    {
        public string AmenitiesJson { get; set; } = "[]"; // Maps to JSON amenities
    }
}
