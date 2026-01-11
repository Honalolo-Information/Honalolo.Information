using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Locations
{
    public class Country : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public int ContinentId { get; set; }
        public Continent Continent { get; set; } = null!;

        // A city has many attractions
        public ICollection<Region> Regions { get; set; } = new List<Region>();
    }
}
