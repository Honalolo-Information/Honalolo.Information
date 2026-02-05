using Honalolo.Information.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Honalolo.Information.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var rootPath = Directory.GetCurrentDirectory(); 
           
            var wwwrootPath = Path.Combine(rootPath, "wwwroot");
            if (!Directory.Exists(wwwrootPath))
            {
               Directory.CreateDirectory(wwwrootPath);
            }

            var uploadsFolder = Path.Combine(wwwrootPath, "uploads", folderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return Task.CompletedTask;

            // path comes in as "/uploads/folder/file.jpg"
            // we need physical path
            var rootPath = Directory.GetCurrentDirectory();
            var wwwrootPath = Path.Combine(rootPath, "wwwroot");
            
            // Remove starting slash if present to combine correctly
            string relativePath = filePath.StartsWith("/") ? filePath.Substring(1) : filePath;
            
            var fullPath = Path.Combine(wwwrootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}