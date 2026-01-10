using Honalolo.Inforamtion.Domain.Common;
using Honalolo.Inforamtion.Domain.Entities.Attractions;
using Honalolo.Inforamtion.Domain.Enums;

public class User : BaseEntity
{
    public UserRole Role { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Attraction> AuthoredAttractions { get; set; } = new List<Attraction>();
}