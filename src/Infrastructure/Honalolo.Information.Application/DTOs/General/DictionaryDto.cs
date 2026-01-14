namespace Honalolo.Information.Application.DTOs.General
{
    // The "Master" object for all dropdowns
    public class DictionaryDto
    {
        public List<SimpleDto> AttractionTypes { get; set; } = new();
        public List<LocationDto> Cities { get; set; } = new();
        public List<SimpleDto> Regions { get; set; } = new();
        public List<SimpleDto> DifficultyLevels { get; set; } = new();
    }

    // A helper for simple ID/Name pairs
    public class SimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Extended helper for Cities to allow filtering by Region on frontend
    public class LocationDto : SimpleDto
    {
        public int ParentId { get; set; } // e.g. RegionId
    }
}