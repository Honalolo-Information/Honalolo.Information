using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Domain.Entities.Attractions;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IAttractionService
    {
        Task<AttractionDetailDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateAttractionDto dto, int userId);
        Task<IEnumerable<AttractionDto>> SearchAsync(AttractionFilterDto filter);
    }
}