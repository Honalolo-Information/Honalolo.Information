using Honalolo.Information.Application.DTOs.Attractions;
using Honalolo.Information.Application.DTOs.Users;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Honalolo.Information.Application.Services
{
    public class UserService : IUserService
    {
        private readonly TouristInfoDbContext _context;

        public UserService(TouristInfoDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.AuthoredAttractions)
                    .ThenInclude(a => a.City) 
                .Include(u => u.AuthoredAttractions)
                    .ThenInclude(a => a.Type)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) throw new Exception("User not found");

            return new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString(),
                MyAttractions = user.AuthoredAttractions.Select(a => new AttractionDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Price = a.Price,
                    CityName = a.City?.Name ?? "Unknown",
                    TypeName = a.Type?.TypeName ?? "Unknown",
                    MainImage = (string.IsNullOrEmpty(a.ImagesJson) || a.ImagesJson == "[]")
                        ? null
                        : JsonSerializer.Deserialize<List<string>>(a.ImagesJson)!.FirstOrDefault()
                }).ToList()
            };
        }

        public async Task UpdateProfileAsync(int userId, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            if (dto.Email != user.Email)
            {
                var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (emailExists) throw new Exception("Email is already taken.");
            }

            user.UserName = dto.UserName;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            {
                throw new Exception("Invalid old password.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();
        }
    }
}