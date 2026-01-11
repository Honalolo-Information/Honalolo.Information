using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Locations
{
    public class Region : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;

        public ICollection<City> City { get; set; } = new List<City>();
    }
}
