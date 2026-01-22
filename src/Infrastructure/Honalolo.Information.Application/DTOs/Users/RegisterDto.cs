using Honalolo.Information.Domain.Enums;

namespace Honalolo.Information.Application.DTOs.Users
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.RegisteredUser; // Default
    }
}
