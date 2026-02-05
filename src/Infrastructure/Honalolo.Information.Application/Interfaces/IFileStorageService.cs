using Microsoft.AspNetCore.Http;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task DeleteFileAsync(string filePath); // Add this method
    }
}