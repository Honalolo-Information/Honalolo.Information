using Honalolo.Information.Application.DTOs.General;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IDictionaryService
    {
        Task<DictionaryDto> GetAllOptionsAsync();
    }
}