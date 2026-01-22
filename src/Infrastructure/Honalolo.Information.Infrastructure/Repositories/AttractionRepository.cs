using Honalolo.Information.Domain.Entities.Attractions;
using Honalolo.Information.Domain.Entities.Interfaces;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Honalolo.Information.Infrastructure.Repositories
{
    internal class AttractionRepository : IAttractionRepository
    {
        private readonly TouristInfoDbContext _context;

        public AttractionRepository(TouristInfoDbContext context)
        {
            _context = context;
        }

        public async Task<Attraction?> GetByIdAsync(int id)
        {
            return await _context.Attractions
                .Include(a => a.OpeningHours)
                .Include(a => a.Languages)
                .Include(a => a.Type)
                .Include(a => a.City)
                .Include(a => a.EventDetails)
                .Include(a => a.TrailDetails) 
                .Include(a => a.HotelDetails) 
                .Include(a => a.FoodDetails)  
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

        public async Task<IEnumerable<Attraction>> SearchAsync(string? searchQuery, string? type, string? city, string? region)
        {
            var query = _context.Attractions
                .Include(a => a.Type)
                .Include(a => a.City)
                .ThenInclude(c => c.Region) // Load parents for filtering
                .AsQueryable();

            // 1. Text Search (Title)
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(a => a.Title.Contains(searchQuery));
            }

            // 2. Filter by Names
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(a => a.Type.TypeName == type);
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(a => a.City.Name == city);
            }

            if (!string.IsNullOrWhiteSpace(region))
            {
                query = query.Where(a => a.City.Region.Name == region);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Attraction>> GetAllAsync(int? typeId, int? regionId, int? cityId, int? countryId, int? continentId)
        {
            var query = _context.Attractions
                .Include(a => a.Type)
                .Include(a => a.City)
                .Include(a => a.City.Region)
                .Include(a => a.City.Region.Country)
                .Include(a => a.City.Region.Country.Continent)
                .AsQueryable();

            if (typeId.HasValue)
                query = query.Where(a => a.TypeId == typeId.Value);

            if (cityId.HasValue)
                query = query.Where(a => a.CityId == cityId.Value);

            if (regionId.HasValue && !cityId.HasValue)
                query = query.Where(a => a.City.RegionId == regionId.Value);

            if (countryId.HasValue && !regionId.HasValue && !cityId.HasValue)
                query = query.Where(a => a.City.Region.CountryId == countryId.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Attraction>> GetByTypeAsync(int typeId)
        {
            return await _context.Attractions
                .Include(a => a.Type)
                .Where(a => a.TypeId == typeId)
                .ToListAsync();
        }

        public async Task<Attraction?> FindAsync(Expression<Func<Attraction, bool>> predicate)
        {
            return await _context.Attractions.FirstOrDefaultAsync(predicate);
        }
    }
}
