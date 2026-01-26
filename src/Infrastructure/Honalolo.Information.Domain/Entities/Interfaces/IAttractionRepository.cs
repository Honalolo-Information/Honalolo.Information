using Honalolo.Information.Domain.Entities.Attractions;

namespace Honalolo.Information.Domain.Entities.Interfaces
{
    public interface IAttractionRepository : IGenericRepository<Attraction>
    {
        Task<IEnumerable<Attraction>> SearchAsync(string? typeName, string? cityName, string? regionName, string? countryName, string? continentName);
    }
}
