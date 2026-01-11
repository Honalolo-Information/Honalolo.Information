using Honalolo.Information.Domain.Entities.Attractions;

namespace Honalolo.Information.Domain.Entities.Interfaces
{
    public interface IAttractionRepository : IGenericRepository<Attraction>
    {
        Task<IEnumerable<Attraction>> GetEventsByRegionAsync(int regionId);
    }
}
