using Honalolo.Inforamtion.Domain.Common;

namespace Honalolo.Inforamtion.Domain.Entities.Locations
{
    public class Continent : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // A city has many attractions
        public ICollection<Country> Countries { get; set; } = new List<Country>();
    }
}
