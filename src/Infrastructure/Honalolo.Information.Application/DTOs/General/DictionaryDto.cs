namespace Honalolo.Information.Application.DTOs.General
{
    public class DictionaryDto
    {
        public List<SimpleDto> AttractionTypes { get; set; } = new();
        public List<LocationDto> Cities { get; set; } = new();
        public List<LocationDto> Regions { get; set; } = new();
        public List<LocationDto> Countries { get; set; } = new();
        public List<SimpleDto> Continents { get; set; } = new();
        public List<SimpleDto> DifficultyLevels { get; set; } = new();
    }

    public class SimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class LocationDto : SimpleDto
    {
        public int ParentId { get; set; }
    }
}