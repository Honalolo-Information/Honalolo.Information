using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Domain.Entities.Attractions;
using Honalolo.Information.Domain.Entities.Interfaces;

namespace Honalolo.Information.Application.Services
{
    public class AttractionService : IAttractionService
    {
        private readonly IAttractionRepository _repository;

        public AttractionService(IAttractionRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(CreateAttractionDto dto, int userId)
        {
            // 1. Map Base Entity
            var attraction = new Attraction
            {
                Title = dto.Title,
                Description = dto.Description,
                CityId = dto.CityId,
                TypeId = dto.TypeId,
                Price = dto.Price,
                AuthorId = userId,
                // Initialize lists to avoid null errors
                OpeningHours = new List<OpeningHour>(),
                Languages = new List<AttractionLanguage>()
            };

            // 2. Handle Specific Sub-Types Logic
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

            // Map sub-objects if they exist
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
                // Convert Enum ID to readable string for Frontend
                var difficultyEnum = (Domain.Enums.DifficultyLevel)entity.TrailDetails.DifficultyLevelId;

                dto.TrailDetails = new TrailDto
                {
                    DistanceMeters = entity.TrailDetails.DistanceMeters,
                    Difficulty = difficultyEnum.ToString()
                };
            }

            return dto;
        }

        public async Task<IEnumerable<AttractionDto>> SearchAsync(AttractionFilterDto filter)
        {
            var entities = await _repository.GetAllAsync(filter.TypeId, filter.RegionId, filter.CityId);

            // Map to DTO (Reuse your existing mapping logic)
            return entities.Select(e => new AttractionDto
            {
                Id = e.Id,
                Title = e.Title,
                CityName = e.City?.Name ?? "",
                TypeName = e.Type?.TypeName ?? "",
                Price = e.Price
            });
        }
    }
}