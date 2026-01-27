using Honalolo.Information.Domain.Common;

namespace Honalolo.Information.Domain.Entities.Reports
{
    public class Report : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public int RequestedByUserId { get; set; }

        public string ParametersJson { get; set; } = string.Empty;

        public string DataJson { get; set; } = string.Empty;
    }
}