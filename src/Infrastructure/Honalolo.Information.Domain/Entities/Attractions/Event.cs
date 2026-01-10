using Honalolo.Inforamtion.Domain.Common;
using Honalolo.Inforamtion.Domain.Entities.Attractions;

namespace Honalolo.Inforamtion.Domain.Entities.Attractions
{
    public class Event : BaseEntity
    {
        public int AttractionId { get; set; } // FK
        public Attraction Attraction { get; set; } = null!;

        public string EventType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
