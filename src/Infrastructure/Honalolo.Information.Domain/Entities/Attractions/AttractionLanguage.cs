using Honalolo.Inforamtion.Domain.Common;

namespace Honalolo.Inforamtion.Domain.Entities.Attractions
{
    public class AttractionLanguage : BaseEntity
    {
        // The language name (e.g., "English", "Deutsch")
        public string LanguageName { get; set; } = string.Empty;

        // Foreign Key Relationship
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; } = null!;
    }
}
