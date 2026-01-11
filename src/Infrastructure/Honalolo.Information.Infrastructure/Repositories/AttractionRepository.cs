using Honalolo.Information.Domain.Entities.Attractions;
using Honalolo.Information.Domain.Entities.Interfaces;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Honalolo.Information.Infrastructure.Repositories
{
    internal class AttractionRepository : IAttractionRepository
    {
        private readonly TouristInfoDbContext _context;

        public AttractionRepository(TouristInfoDbContext context)
        {
            _context = context;
        }

        // Standard CRUD
        public async Task<Attraction?> GetByIdAsync(int id)
        {
            return await _context.Attractions
                .Include(a => a.OpeningHours) // Load child data eagerly
                .Include(a => a.Languages)
                .Include(a => a.Type)
                .Include(a => a.EventDetails) // Load extension if it exists
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Attraction attraction)
        {
            await _context.Attractions.AddAsync(attraction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attraction attraction)
        {
            _context.Attractions.Update(attraction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction != null)
            {
                _context.Attractions.Remove(attraction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Attraction>> GetEventsByRegionAsync(int regionId)
        {
            return await _context.Attractions
                .Include(a => a.EventDetails) // We only care about Events
                .Include(a => a.City)
                .Where(a => a.City.RegionId == regionId) // Filter by Region Hierarchy
                .Where(a => a.EventDetails != null)      // Only return rows that ARE events
                .ToListAsync();
        }
        public async Task<IEnumerable<Attraction>> GetAllAsync()
        {
            return await _context.Attractions
                .Include(a => a.Type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attraction>> GetByTypeAsync(int typeId)
        {
            return await _context.Attractions
                .Include(a => a.Type)
                .Where(a => a.TypeId == typeId)
                .ToListAsync();
        }
    }
}
