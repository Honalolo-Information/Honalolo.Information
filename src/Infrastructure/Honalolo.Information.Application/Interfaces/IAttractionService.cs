using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Domain.Entities.Attractions;
using Microsoft.AspNetCore.Http;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IAttractionService
    {
        Task<AttractionDetailDto?> GetByIdAsync(int id);
        Task<AttractionDetailDto> CreateAsync(CreateAttractionDto dto, int userId);
        Task<IEnumerable<AttractionDto>> SearchAsync(AttractionFilterDto filter);
        Task<AttractionDetailDto?> AddPhotosAsync(int attractionId, List<IFormFile> files, int userId);
    }
}