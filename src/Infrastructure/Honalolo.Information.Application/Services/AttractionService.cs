using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Domain.Entities.Attractions;
using Honalolo.Information.Domain.Entities.Interfaces;
using Honalolo.Information.Domain.Entities.Locations;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Honalolo.Information.Application.Services
{
    public class AttractionService : IAttractionService
    {
        private readonly IAttractionRepository _repository;
        private readonly IFileStorageService _fileStorage;

        private readonly TouristInfoDbContext _context;

        public AttractionService(IAttractionRepository repository, TouristInfoDbContext context, IFileStorageService fileStorage)
        {
            _repository = repository;
            _context = context;
            _fileStorage = fileStorage;
        }

        public async Task<AttractionDetailDto> CreateAsync(CreateAttractionDto dto, int userId)
        {
            // Fix 1: Find AttractionType by TypeName (string), not by ID (int)
            var attractionType = await _context.AttractionTypes
                .FirstOrDefaultAsync(t => t.TypeName == dto.TypeName);

            if (attractionType == null)
                throw new ArgumentException($"Invalid Attraction Type: {dto.TypeName}");

            // Fix 2: Resolve Location correctly (String -> Entity -> Id)
            var city = await GetOrCreateLocationChainAsync(dto);

            // Fix 3: Create Attraction base entity first (Composition over Inheritance logic)
            var attraction = new Attraction
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                CityId = city.Id,      // Use resolved City ID
                TypeId = attractionType.Id, // Use resolved Type ID
                AuthorId = userId,
                ImagesJson = "[]", // Default empty JSON array
                OpeningHours = dto.OpeningHours.Select(oh => new OpeningHour { Content = oh }).ToList(),
                Languages = dto.Languages.Select(l => new AttractionLanguage { LanguageName = l }).ToList()
            };

            // Fix 4: Attach related details based on TypeName
            switch (attractionType.TypeName)
            {
                case "Event":
                    if (dto.EventDetails == null)
                        throw new ArgumentException("Event details are required for type 'Event'");

                    attraction.EventDetails = new Event
                    {
                        StartDate = dto.EventDetails.StartDate,
                        EndDate = dto.EventDetails.EndDate,
                        EventType = "General" // Default value as it's required but missing in DTO
                    };
                    break;

                case "Trail":
                    if (dto.TrailDetails != null)
                    {
                        attraction.TrailDetails = new Trail
                        {
                            DistanceMeters = dto.TrailDetails.DistanceMeters,
                            DifficultyLevelId = dto.TrailDetails.DifficultyLevelId,
                            StartingPoint = "Unknown", // Required in Entity, missing in DTO
                            EndpointPoint = "Unknown"  // Required in Entity, missing in DTO
                        };
                    }
                    break;

                case "Hotel":
                    if (dto.HotelDetails != null)
                    {
                        attraction.HotelDetails = new Hotel
                        {
                            AmenitiesJson = dto.HotelDetails.Amenities
                        };
                    }
                    break;

                case "Restaurant":
                    if (dto.FoodDetails != null)
                    {
                        attraction.FoodDetails = new Food
                        {
                            CuisineType = dto.FoodDetails.FoodType
                        };
                    }
                    break;
                
                // For "Museum", "Park" etc, no extra details needed or not implemented yet.
            }

            // 4. Save - EF Core will insert Attraction and the attached related entity (Event/Trail/etc.)
            await _repository.AddAsync(attraction);

            return await GetByIdAsync(attraction.Id);
        }

        public async Task<IEnumerable<AttractionDto>> SearchAsync(AttractionFilterDto filter)
        {
            // 1. Get Raw Entities from Repository
            var entities = await _repository.SearchAsync(
                filter.TypeName,
                filter.CityName,
                filter.RegionName,
                filter.CountryName,
                filter.ContinentName
            );

            // 2. Map Entity -> DTO (Breaking the cycle)
            var dtos = entities.Select(e => new AttractionDto
            {
                
                Id = e.Id,
                Title = e.Title,
                Price = e.Price,
                AuthorId = e.AuthorId, // Added AuthorId mapping
                MainImage = (string.IsNullOrEmpty(e.ImagesJson) || e.ImagesJson == "[]")
                    ? null
                    : JsonSerializer.Deserialize<List<string>>(e.ImagesJson)!.FirstOrDefault(),

                CityName = e.City?.Name ?? "Unknown",
                TypeName = e.Type?.TypeName ?? "Unknown",
                OpeningHours = e.OpeningHours != null ? e.OpeningHours.Select(o => o.Content).ToList() : new List<string>(),

                // Map specific details
                EventStartDate = e.EventDetails?.StartDate,
                EventEndDate = e.EventDetails?.EndDate,
                TrailDifficulty = e.TrailDetails != null ? ((Domain.Enums.DifficultyLevel)e.TrailDetails.DifficultyLevelId).ToString() : null,
                FoodType = e.FoodDetails?.CuisineType,
                HotelAmenities = e.HotelDetails?.AmenitiesJson
            });

            return dtos;
        }

        public async Task<AttractionDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var images = (string.IsNullOrEmpty(entity.ImagesJson) || entity.ImagesJson == "[]")
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(entity.ImagesJson) ?? new List<string>();

            var dto = new AttractionDetailDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CityName = entity.City?.Name ?? "Unknown",
                TypeName = entity.Type?.TypeName ?? "Unknown",
                Price = entity.Price,
                AuthorId = entity.AuthorId, // Added AuthorId mapping
                Languages = entity.Languages.Select(l => l.LanguageName).ToList(),
                OpeningHours = entity.OpeningHours.Select(o => o.Content).ToList(),
                Images = images,
                MainImage = images.FirstOrDefault() ?? string.Empty
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

            if (entity.FoodDetails != null)
            {
                dto.FoodDetails = new FoodDto
                {
                    FoodType = entity.FoodDetails.CuisineType
                };
            }

            if (entity.HotelDetails != null)
            {
                dto.HotelDetails = new HotelDto
                {
                    Amenities = entity.HotelDetails.AmenitiesJson
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

        public async Task<AttractionDetailDto?> AddPhotosAsync(int attractionId, List<IFormFile> files, int userId)
        {
            var attraction = await _repository.GetByIdAsync(attractionId);
            if (attraction == null) return null;

            // Prosta walidacja: tylko autor lub admin może dodać zdjęcia
            var user = await _context.Users.FindAsync(userId);
            if (attraction.AuthorId != userId && user?.Role != Domain.Enums.UserRole.Administrator)
            {
                throw new UnauthorizedAccessException("Brak uprawnień do edycji tej atrakcji.");
            }

            var newPaths = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Używamy naszego serwisu do zapisu
                    var path = await _fileStorage.SaveFileAsync(file, "attractions");
                    newPaths.Add(path);
                }
            }

            // Aktualizacja JSON-a w bazie
            var currentImages = string.IsNullOrEmpty(attraction.ImagesJson) || attraction.ImagesJson == "[]"
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(attraction.ImagesJson) ?? new List<string>();

            currentImages.AddRange(newPaths);
            attraction.ImagesJson = JsonSerializer.Serialize(currentImages);

            await _repository.UpdateAsync(attraction);

            return await GetByIdAsync(attractionId);
        }

        public async Task<AttractionDetailDto?> UpdateAsync(int id, UpdateAttractionDto dto, int userId, bool isAdmin)
        {
            var attraction = await _context.Attractions
                .Include(a => a.Type)
                .Include(a => a.EventDetails)
                .Include(a => a.TrailDetails)
                .Include(a => a.HotelDetails)
                .Include(a => a.FoodDetails)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attraction == null) return null;

            // Authorization: Author or Admin
            if (attraction.AuthorId != userId && !isAdmin)
            {
                throw new UnauthorizedAccessException("You are not allowed to edit this attraction.");
            }

            // Update basic fields
            attraction.Title = dto.Title;
            attraction.Description = dto.Description;
            attraction.Price = dto.Price;

            // Update Location if provided (assuming validation is handled elsewhere or via similar logic to Create)
            if (!string.IsNullOrEmpty(dto.CityName) && !string.IsNullOrEmpty(dto.RegionName) && !string.IsNullOrEmpty(dto.CountryName) && !string.IsNullOrEmpty(dto.ContinentName))
            {
                // Re-use logic to resolve city
                // Note: Ideally, this logic should be extracted to avoid duplication, but for now we inline or call similar methods
                 var city = await GetOrCreateLocationChainAsync(new CreateAttractionDto 
                 { 
                     CityName = dto.CityName, 
                     RegionName = dto.RegionName, 
                     CountryName = dto.CountryName, 
                     ContinentName = dto.ContinentName 
                 });
                 attraction.CityId = city.Id;
            }


            // Update specific details based on Type
            switch (attraction.Type.TypeName)
            {
                case "Event":
                    if (dto.EventDetails != null && attraction.EventDetails != null)
                    {
                        attraction.EventDetails.StartDate = dto.EventDetails.StartDate;
                        attraction.EventDetails.EndDate = dto.EventDetails.EndDate;
                    }
                    else if (dto.EventDetails != null && attraction.EventDetails == null)
                    {
                         attraction.EventDetails = new Event
                        {
                            StartDate = dto.EventDetails.StartDate,
                            EndDate = dto.EventDetails.EndDate,
                            EventType = "General"
                        };
                    }
                    break;

                case "Trail":
                    if (dto.TrailDetails != null && attraction.TrailDetails != null)
                    {
                        attraction.TrailDetails.DistanceMeters = dto.TrailDetails.DistanceMeters;
                        attraction.TrailDetails.DifficultyLevelId = dto.TrailDetails.DifficultyLevelId;
                    }
                    else if (dto.TrailDetails != null && attraction.TrailDetails == null)
                    {
                        attraction.TrailDetails = new Trail
                        {
                            DistanceMeters = dto.TrailDetails.DistanceMeters,
                            DifficultyLevelId = dto.TrailDetails.DifficultyLevelId,
                            StartingPoint = "Unknown",
                            EndpointPoint = "Unknown"
                        };
                    }
                    break;

                case "Hotel":
                    if (dto.HotelDetails != null && attraction.HotelDetails != null)
                    {
                        attraction.HotelDetails.AmenitiesJson = dto.HotelDetails.Amenities;
                    }
                    else if (dto.HotelDetails != null && attraction.HotelDetails == null)
                    {
                         attraction.HotelDetails = new Hotel
                        {
                            AmenitiesJson = dto.HotelDetails.Amenities
                        };
                    }
                    break;

                case "Restaurant":
                    if (dto.FoodDetails != null && attraction.FoodDetails != null)
                    {
                        attraction.FoodDetails.CuisineType = dto.FoodDetails.FoodType;
                    }
                     else if (dto.FoodDetails != null && attraction.FoodDetails == null)
                    {
                         attraction.FoodDetails = new Food
                        {
                            CuisineType = dto.FoodDetails.FoodType
                        };
                    }
                    break;
            }

            await _repository.UpdateAsync(attraction);

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id, int userId, bool isAdmin)
        {
            var attraction = await _repository.GetByIdAsync(id);
            if (attraction == null) return false;

            // Authorization: ONLY Admin can delete
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Only administrators can delete attractions.");
            }

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}