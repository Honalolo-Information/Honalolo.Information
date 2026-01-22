using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Attractions.Honalolo.Information.Application.DTOs.Attractions;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IAttractionService
    {
        Task<AttractionDetailDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateAttractionDto dto, int userId);
    }
}