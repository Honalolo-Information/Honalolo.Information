using Honalolo.Inforamtion.Domain.Common;

namespace Honalolo.Inforamtion.Domain.Entities.Attractions
{
    public class Hotel : BaseEntity
    {
        public string AmenitiesJson { get; set; } = "[]"; // Maps to JSON amenities
    }
}
