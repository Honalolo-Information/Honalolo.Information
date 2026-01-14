using Honalolo.Information.Application.DTOs.General;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Domain.Enums;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Honalolo.Information.Application.Services
{
    public class DictionaryService : IDictionaryService
    {
        private readonly TouristInfoDbContext _context;

        public DictionaryService(TouristInfoDbContext context)
        {
            _context = context;
        }

        public async Task<DictionaryDto> GetAllOptionsAsync()
        {
            var response = new DictionaryDto();

            // 1. Fetch Types
            response.AttractionTypes = await _context.AttractionTypes
                .Select(x => new SimpleDto { Id = x.Id, Name = x.TypeName })
                .ToListAsync();

            // 2. Fetch Regions
            response.Regions = await _context.Regions
                .Select(x => new SimpleDto { Id = x.Id, Name = x.Name })
                .ToListAsync();

            // 3. Fetch Cities (Including RegionId so frontend can cascade filters)
            response.Cities = await _context.Cities
                .Select(x => new LocationDto { Id = x.Id, Name = x.Name, ParentId = x.RegionId })
                .ToListAsync();

            // 4. Manual Map for Enums (Difficulty Level)
            response.DifficultyLevels = Enum.GetValues(typeof(DifficultyLevel))
                .Cast<DifficultyLevel>()
                .Select(e => new SimpleDto { Id = (int)e, Name = e.ToString() })
                .ToList();

            return response;
        }
    }
}