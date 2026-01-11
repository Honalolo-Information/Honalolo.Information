using Honalolo.Information.Domain.Common;
using Honalolo.Information.Domain.Enums;
using Honalolo.Information.Domain.Entities.Attractions;

public class User : BaseEntity
{
    public UserRole Role { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Attraction> AuthoredAttractions { get; set; } = new List<Attraction>();
}