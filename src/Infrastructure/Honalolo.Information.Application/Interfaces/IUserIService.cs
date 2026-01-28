using Honalolo.Information.Application.DTOs.Users;

namespace Honalolo.Information.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfileAsync(int userId);
        Task UpdateProfileAsync(int userId, UpdateUserDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}