using Honalolo.Information.Domain.Common;
using Honalolo.Information.Domain.Entities.Attractions;

namespace Honalolo.Information.Domain.Entities.Locations
{
    public class City : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public int RegionId { get; set; }
        public Region Region { get; set; } = null!;

        // A city has many attractions
        public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
    }
}
