using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Domain.Entities.Attractions;
using Honalolo.Information.Domain.Entities.Interfaces;
using Honalolo.Information.Domain.Entities.Locations;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Honalolo.Information.Application.Services
{
    public class AttractionService : IAttractionService
    {
        private readonly IAttractionRepository _repository;

        private readonly TouristInfoDbContext _context;

        public AttractionService(IAttractionRepository repository, TouristInfoDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<int> CreateAsync(CreateAttractionDto dto, int userId)
        {
            // A. Handle Attraction Type (Find or Create)
            var type = await _context.AttractionTypes
                .FirstOrDefaultAsync(t => t.TypeName == dto.TypeName);

            if (type == null)
            {
                type = new AttractionType { TypeName = dto.TypeName };
                _context.AttractionTypes.Add(type);
                await _context.SaveChangesAsync();
            }

            // B. Handle Location Hierarchy (Find or Create Chain)
            var city = await GetOrCreateLocationChainAsync(dto);

            // C. Create Attraction
            var attraction = new Attraction
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                AuthorId = userId,
                TypeId = type.Id,     // Use the resolved ID
                CityId = city.Id,     // Use the resolved ID
                OpeningHours = new List<OpeningHour>(),
                Languages = new List<AttractionLanguage>()
            };

            if (dto.EventDetails != null)
            {
                attraction.EventDetails = new Event
                {
                    StartDate = dto.EventDetails.StartDate,
                    EndDate = dto.EventDetails.EndDate,
                    EventType = "General"
                };
            }
            else if (dto.TrailDetails != null)
            {
                attraction.TrailDetails = new Trail
                {
                    DistanceMeters = dto.TrailDetails.DistanceMeters,
                    DifficultyLevelId = dto.TrailDetails.DifficultyLevelId,
                };
            }
            else if (dto.HotelDetails != null)
            {
                attraction.HotelDetails = new Hotel
                {
                    AmenitiesJson = dto.HotelDetails.Amenities
                };
            }
            else if (dto.FoodDetails != null)
            {
                attraction.FoodDetails = new Food
                {
                    CuisineType = dto.FoodDetails.FoodType
                };
            }

            await _repository.AddAsync(attraction);
            return attraction.Id;
        }

        public async Task<AttractionDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var dto = new AttractionDetailDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CityName = entity.City?.Name ?? "Unknown",
                TypeName = entity.Type?.TypeName ?? "Unknown",
                Price = entity.Price,
                Languages = entity.Languages.Select(l => l.LanguageName).ToList(),
                OpeningHours = entity.OpeningHours.Select(o => o.Content).ToList(),
            };

            if (entity.EventDetails != null)
            {
                dto.EventDetails = new EventDto
                {
                    StartDate = entity.EventDetails.StartDate,
                    EndDate = entity.EventDetails.EndDate
                };
            }

            if (entity.TrailDetails != null)
            {
                var difficultyEnum = (Domain.Enums.DifficultyLevel)entity.TrailDetails.DifficultyLevelId;

                dto.TrailDetails = new TrailDto
                {
                    DistanceMeters = entity.TrailDetails.DistanceMeters,
                    Difficulty = difficultyEnum.ToString()
                };
            }

            return dto;
        }

        private async Task<City> GetOrCreateLocationChainAsync(CreateAttractionDto dto)
        {
            // 1. Continent
            var continent = await _context.Continents.FirstOrDefaultAsync(c => c.Name == dto.ContinentName);
            if (continent == null)
            {
                continent = new Continent { Name = dto.ContinentName };
                _context.Continents.Add(continent);
                await _context.SaveChangesAsync();
            }

            // 2. Country
            var country = await _context.Countries
                .FirstOrDefaultAsync(c => c.Name == dto.CountryName && c.ContinentId == continent.Id);
            if (country == null)
            {
                country = new Country { Name = dto.CountryName, ContinentId = continent.Id };
                _context.Countries.Add(country);
                await _context.SaveChangesAsync();
            }

            // 3. Region
            var region = await _context.Regions
                .FirstOrDefaultAsync(r => r.Name == dto.RegionName && r.CountryId == country.Id);
            if (region == null)
            {
                region = new Region { Name = dto.RegionName, CountryId = country.Id };
                _context.Regions.Add(region);
                await _context.SaveChangesAsync();
            }

            // 4. City
            var city = await _context.Cities
                .FirstOrDefaultAsync(c => c.Name == dto.CityName && c.RegionId == region.Id);
            if (city == null)
            {
                city = new City { Name = dto.CityName, RegionId = region.Id };
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();
            }

            return city;
        }
    }
}