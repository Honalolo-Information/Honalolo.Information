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
    }
}