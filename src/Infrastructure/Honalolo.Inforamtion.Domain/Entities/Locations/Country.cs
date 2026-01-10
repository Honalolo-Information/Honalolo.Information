using Honalolo.Inforamtion.Domain.Common;

namespace Honalolo.Inforamtion.Domain.Entities.Locations
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
