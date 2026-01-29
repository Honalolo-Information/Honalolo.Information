namespace Honalolo.Information.Application.DTOs.Attractions
{
    public class AttractionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty; // Flattened from City object
        public string TypeName { get; set; } = string.Empty; // Flattened from Type object
        public decimal Price { get; set; }
        public string MainImage { get; set; } = string.Empty; // Extracted from JSON
        public int AuthorId { get; set; }
    }
}
