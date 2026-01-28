using Honalolo.Information.Application.DTOs.Attractions;

namespace Honalolo.Information.Application.DTOs.Users
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public List<AttractionDto> MyAttractions { get; set; } = new();
    }
}