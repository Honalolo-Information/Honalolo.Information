using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Locations
{
    public class Continent : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Country> Countries { get; set; } = new List<Country>();
    }
}
