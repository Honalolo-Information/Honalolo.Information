using Honalolo.Information.Domain.Entities.Attractions;

namespace Honalolo.Information.Domain.Entities.Interfaces
{
    public interface IAttractionRepository : IGenericRepository<Attraction>
    {
        Task<IEnumerable<Attraction>> SearchAsync(string? searchQuery, string? type, string? city, string? region);
    }
}
