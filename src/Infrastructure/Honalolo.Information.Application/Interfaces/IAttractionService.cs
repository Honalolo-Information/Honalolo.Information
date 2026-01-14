using Honalolo.Information.Application.DTOs.Attractions;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IAttractionService
    {
        Task<IEnumerable<AttractionDto>> SearchAsync(AttractionFilterDto filter);
        Task<AttractionDetailDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateAttractionDto dto, int userId);
    }
}