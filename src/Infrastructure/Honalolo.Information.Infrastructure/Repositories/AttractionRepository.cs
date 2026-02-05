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
                    .ThenInclude(c => c.Region)
                    .ThenInclude(r => r.Country)
                    .ThenInclude(c => c.Continent)
                .Include(a => a.EventDetails)
                .Include(a => a.TrailDetails) 
                .Include(a => a.HotelDetails) 
                .Include(a => a.FoodDetails)  
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Attraction>> SearchAsync(string? type, string? city, string? region, string? country, string? continent)
        {
            // Start with the base query and include necessary relationships
            var query = _context.Attractions
                .Include(a => a.Type)
                .Include(a => a.City)
                    .ThenInclude(c => c.Region)
                    .ThenInclude(r => r.Country)
                    .ThenInclude(r => r.Continent)
                .Include(a => a.OpeningHours)
                .Include(a => a.EventDetails)
                .Include(a => a.TrailDetails)
                .Include(a => a.HotelDetails)
                .Include(a => a.FoodDetails)
                .AsQueryable();

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

            if (!string.IsNullOrWhiteSpace(country))
            {
                query = query.Where(a => a.City.Region.Country.Name == country);
            }

            if (!string.IsNullOrWhiteSpace(continent))
            {
                query = query.Where(a => a.City.Region.Country.Continent.Name == continent);
            }

            return await query.ToListAsync();
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

        public async Task<Attraction?> FindAsync(Expression<Func<Attraction, bool>> predicate)
        {
            return await _context.Attractions.FirstOrDefaultAsync(predicate);
        }
    }
}
